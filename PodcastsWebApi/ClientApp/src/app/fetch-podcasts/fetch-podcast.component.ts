import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatDialog, MatDialogRef } from '@angular/material';
import { TextInputDialogComponent } from '../text-input-dialog/text-input-dialog.component';

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
    http.get<Podcast[]>(baseUrl + 'api/podcasts').subscribe(result => {
      this.podcasts = result;
    }, error => console.error(error));

    this.httpContext = http;
    this.baseUrl = baseUrl; 
  }

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

  findPodcast() {
    var dialogRef: MatDialogRef<TextInputDialogComponent> = this.dialog.open(TextInputDialogComponent);
    dialogRef.componentInstance.title = 'Custom Podcast URL';
    dialogRef.componentInstance.message = 'eg: https://www.reddit.com/.rss';
    dialogRef.componentInstance.onOk.subscribe(result => {
      console.log(result);
      result = result.replace("https://", "");
      result = result.replace("http://", "");

      const CORS_PROXY = "https://cors-anywhere.herokuapp.com/";
      let parser = new RSSParser();
      parser.parseURL(CORS_PROXY + result, (err, feed) => {
        if (err) throw err;
        console.log(feed);

        var newPodcast = {
          title: feed.title,
          url: feed.feedUrl,
          numberOfEpisodes: feed.items.length,
          picture: feed.image.url,
          author: feed.managingEditor
        };

        this.httpContext.post<Podcast>(this.baseUrl + 'api/podcasts', newPodcast)
          .subscribe(
            result => {
              console.log(result);
              this.podcasts.push(result);
            },
            error => {
              console.error(error);
            }); 

      });

    });
  }
}

interface Podcast {
  id: number;
  title: string;
  url: string;
  author: string;
  numberOfEpisodes: number;
  picture: string;
}
