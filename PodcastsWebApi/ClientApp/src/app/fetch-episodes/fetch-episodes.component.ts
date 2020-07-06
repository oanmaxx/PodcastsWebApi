import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Podcast } from '../fetch-podcasts/fetch-podcast.component';
import { ActivatedRoute, Router } from '@angular/router';
import { HomeComponent } from '../home/home.component';
import { MatDialog, MatDialogRef } from '@angular/material';
import { ConfirmationDialog } from '../confirm-dialog/confirmation-dialog';

@Component({
  selector: 'app-fetch-episodes',
  templateUrl: './fetch-episodes.component.html',
  styleUrls: ['./fetch-episodes.component.css'],
})
export class FetchEpisodesComponent {
  private static lastSelectedPodcastId: number;
  public episodes: Episodes[];
  public podcastTitle: string;
  public podcastId: number;
  private httpContext: HttpClient;
  private baseUrl: string;

  constructor(
    http: HttpClient,
    route: ActivatedRoute,
    @Inject('BASE_URL') baseUrl: string,
    router: Router,
    public confirmDialog: MatDialog) {

    if (HomeComponent.GetLoggedInUser() == null) {
      router.navigate(['/']);
    }

    this.httpContext = http;
    this.baseUrl = baseUrl;

    let podcastId = route.snapshot.queryParams['podcast'] ? route.snapshot.queryParams['podcast'] : 0;
    if (!podcastId) {
      podcastId = FetchEpisodesComponent.lastSelectedPodcastId;
    } else {
      FetchEpisodesComponent.lastSelectedPodcastId = podcastId;
    }

    this.podcastId = 0;

    if (podcastId) {
      this.httpContext.get<Podcast>(this.baseUrl + 'api/podcasts/' + podcastId).subscribe(
        result => {
          this.podcastTitle = result.title;
          this.podcastId = result.id;

          this.readEpisodes(this.podcastId, "");
        },
        error => {
          console.error(error)
        });
    } else {
      this.podcastTitle = "There is no active podcast selected.";
    }

    this.readEpisodes(this.podcastId, "");
  }

  // CRUD - read
  public readEpisodes(podcastId: number, filter: string) {
    if (podcastId > 0) {
      if (filter.length == 0) {
        this.httpContext.get<Episodes[]>(this.baseUrl + 'api/episodes/podcastepisodes/' + podcastId).subscribe(result => {
          this.episodes = result;
        }, error => console.error(error));
      } else {
        this.httpContext.get<Episodes[]>(this.baseUrl + 'api/episodes/podcastepisodesfilter/' + podcastId + "/" + filter).subscribe(result => {
          this.episodes = result;
        }, error => console.error(error));
      }
    } else {
      this.episodes = [];
    }
  }

  public onSearchEpisode(filter: string) {
    this.readEpisodes(this.podcastId, filter);
  }

  // CRUD - delete
  public removeEpisodeConfirm(inputId: number) {
    var dialogConfirmRef: MatDialogRef<ConfirmationDialog> = this.confirmDialog.open(ConfirmationDialog);
    dialogConfirmRef.componentInstance.confirmMessage = 'Remove episode?';
    dialogConfirmRef.afterClosed().subscribe(result => {
      if (result) {
        this.removeEpisode(inputId);
      }
    });
  }

  private removeEpisode(inputId: number) {
    this.httpContext
      .delete<number>(this.baseUrl + 'api/episodes/' + inputId)
      .subscribe(
        result => {
          console.log(result);
          var removedIndex = this.episodes.findIndex(a => a.id == inputId);
          this.episodes.splice(removedIndex, 1);
        },
        error => {
          console.error(error)
        });
  }

  // CRUD - create
  public static createEpisodes(http: HttpClient, apiurl: string, podcastId: number, feed) {    
    var allEpisodes = this.readEpisodes(podcastId, feed);

    for (var i = 0; i < allEpisodes.length; i++) {
      let episode = allEpisodes[i] as Episodes;
      http.post<Episodes>(apiurl + 'api/episodes', episode)
        .subscribe(
          result => {
            console.log(result);
          },
          error => {
            console.error(error);
          });
    }
  }

  private static readEpisodes(inputpodcastid:number, items: Episodes[]): Episodes[] {
    var result = [];
    for (var i = 0; i < items.length; i++) {
      var episode = items[i] as Episodes;

      var pictureUrl = episode.picture ? episode.picture : '';
      var author = episode.author != null ? episode.author : 'Mixed';
      var descriptionTrim = episode.content.length > 100
        ? episode.content.slice(0, 100) + "..."
        : episode.content;
      var newEpisode = {
        id: 0,
        podcastid: inputpodcastid,
        title: episode.title,
        description: descriptionTrim,
        author: author,
        picture: pictureUrl,
        link: episode.link,
        pubDate: episode.pubDate
      };

      result.push(newEpisode);
    }

    return result;
  }

  // CRUD - update
  public static updateEpisodes(http: HttpClient, apiurl: string, podcastId: number, feed) {    
    http.get<Episodes[]>(apiurl + 'api/episodes/podcastepisodes/' + podcastId).subscribe(
      result => {
        let existingEpisodes = result;
        let incomingEpisodes = this.readEpisodes(podcastId, feed);

        console.log("existing episodes:");
        console.log(existingEpisodes);

        console.log("incoming episodes:");
        console.log(incomingEpisodes);

        var deletedEpisodes = existingEpisodes.filter(function ($value) {
          let foundEpisode = incomingEpisodes.find(e => e.title === $value.title);
          return foundEpisode == null;
        });

        var createEpisodes = incomingEpisodes.filter(function ($value) {
          let foundEpisode = existingEpisodes.find(e => e.title === $value.title);
          return foundEpisode == null;
        });

        var updateEpisodes = incomingEpisodes.filter(function ($value) {
          let foundEpisode = existingEpisodes.find(e => e.title === $value.title);          
          return foundEpisode != null;
        });

        console.log("Episodes to be deleted:");
        console.log(deletedEpisodes);

        console.log("Episodes to be created:");
        console.log(createEpisodes);

        console.log("Episodes to be updated:");
        console.log(updateEpisodes);

        for (var i = 0; i < deletedEpisodes.length; i++) {
          let episode = deletedEpisodes[i] as Episodes;
          http.delete<Episodes>(apiurl + 'api/episodes/' + episode.id)
            .subscribe(
              result => { console.log('Deleted episode: ' + result.title); },
              error => {
                console.error(error)
              });
        }

        for (var i = 0; i < createEpisodes.length; i++) {
          let episode = createEpisodes[i] as Episodes;
          http.post<Episodes>(apiurl + 'api/episodes', episode)
            .subscribe(
              result => {
                console.log('Created episode: ' + result.title);
              },
              error => {
                console.error(error);
              });
        }

        let header = new HttpHeaders();
        header.append('Content-Type', 'application/json');
        for (var i = 0; i < updateEpisodes.length; i++) {
          let episode = updateEpisodes[i] as Episodes;

          let foundEpisode = existingEpisodes.find(e => e.title === episode.title);
          episode.id = foundEpisode.id;

          http.put<Episodes>(apiurl + 'api/episodes/' + foundEpisode.id, episode, { headers: header })
            .subscribe(
              result => {
                console.log('Updated episode: ' + result.title);
              },
              error => {
                console.error(error);
              });
        }
      },
      error => {
        console.error(error);
      });
  }

  // Play media
  public playEpisode(url: string) {
    var win = window.open(url, '_blank');
    win.focus();
  }
}

interface Episodes {
  id: number;
  podcastid: number;
  title: string;
  description: string;  
  author: string;
  picture: string;
  link: string;
  pubDate: string;  

  //item
  content: string;
}
