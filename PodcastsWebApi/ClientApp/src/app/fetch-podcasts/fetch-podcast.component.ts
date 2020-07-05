import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatDialog, MatDialogRef } from '@angular/material';
import { TextInputDialogComponent } from '../text-input-dialog/text-input-dialog.component';
import { FetchEpisodesComponent } from '../fetch-episodes/fetch-episodes.component';

// This tells TypeScript that you know this will be available globally during runtime.
// Unfortunately, RSSParser has no TS Declarations, so you have to use any or make a more concrete type yourself
declare const RSSParser: any;

@Component({
  selector: 'app-fetch-podcast',
  templateUrl: './fetch-podcast.component.html'
})
export class FetchPodcastComponent {
  public podcasts: Podcast[];
  private httpContext: HttpClient;
  private baseUrl: string;

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    public dialog: MatDialog) {
   
    this.httpContext = http;
    this.baseUrl = baseUrl;

    this.readPodcasts();
  }

  // CRUD - read
  readPodcasts() {
    this.httpContext.get<Podcast[]>(this.baseUrl + 'api/podcasts').subscribe(result => {
      this.podcasts = result;
    }, error => console.error(error));
  }

  // CRUD - delete
  removePodcast(inputId: number) {
    this.httpContext
      .delete<number>(this.baseUrl + 'api/podcasts/' + inputId)
      .subscribe(
        result => {
          console.log(result);
          var removedIndex = this.podcasts.findIndex(a => a.id == inputId);
          this.podcasts.splice(removedIndex, 1);
        },
        error => {
          console.error(error)
        });
  }

  // CRUD - create
  findPodcast() {
    var dialogRef: MatDialogRef<TextInputDialogComponent> = this.dialog.open(TextInputDialogComponent);
    dialogRef.componentInstance.title = 'Custom Podcast URL';
    dialogRef.componentInstance.message = 'eg: https://www.reddit.com/.rss';
    dialogRef.componentInstance.onOk.subscribe(result => {
      console.log(result);
      var url = result.replace("https://", "");
      url = url.replace("http://", "");

      const CORS_PROXY = "https://cors-anywhere.herokuapp.com/";
      let parser = new RSSParser();
      parser.parseURL(CORS_PROXY + url, (err, feed) => {
        if (err) throw err;
        console.log(feed);

        this.createPodcast(feed);
      });
    });
  }

  private createPodcast(feed) {

    var newPodcast = this.readPodcastInfo(feed);

    this.httpContext.post<Podcast>(this.baseUrl + 'api/podcasts', newPodcast)
      .subscribe(
        result => {
          console.log(result);
          this.podcasts.push(result);

          FetchEpisodesComponent.createEpisodes(this.httpContext, this.baseUrl, result.id, feed.items);
        },
        error => {
          console.error(error);
        });
  }

  private readPodcastInfo(feed): Podcast {
    var pictureUrl = feed.image ? feed.image.url : '';
    var author = feed.managingEditor != null ? feed.managingEditor : 'Mixed';

    var newPodcast = {
      id: 0,
      title: feed.title,
      url: feed.feedUrl,
      numberOfEpisodes: feed.items.length,
      picture: pictureUrl,
      author: author
    };

    return newPodcast;
  }

  // CRUD - update
  public refreshPodcast(inputId: number) {
    var index = this.podcasts.findIndex(a => a.id == inputId);
    var url = this.podcasts[index].url;
    url = url.replace("https://", "");
    url = url.replace("http://", "");

    const CORS_PROXY = "https://cors-anywhere.herokuapp.com/";
    let parser = new RSSParser();
    parser.parseURL(CORS_PROXY + url, (err, feed) => {
      if (err) throw err;
      console.log(feed);

      this.updatePodcast(inputId, index, feed);
    });
  }

  private updatePodcast(inputId: number, index: number, feed) {
    var updatePodcast = this.readPodcastInfo(feed);
    updatePodcast.id = inputId;

    let header = new HttpHeaders();
    header.append('Content-Type', 'application/json');

    let url = `${this.baseUrl}api/podcasts/${updatePodcast.id}`;

    this.httpContext
      .put<Podcast>(url, updatePodcast, { headers: header })
      .subscribe(
        result => {
          console.log(result);
          this.podcasts[index] = updatePodcast;

          FetchEpisodesComponent.updateEpisodes(this.httpContext, this.baseUrl, result.id, feed.items);
        },
        error => {
          console.error(error);
        });
  }
}

export interface Podcast {
  id: number;
  title: string;
  url: string;
  author: string;
  numberOfEpisodes: number;
  picture: string;
}
