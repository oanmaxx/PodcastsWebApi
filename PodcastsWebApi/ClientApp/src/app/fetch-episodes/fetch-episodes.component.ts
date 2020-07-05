import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Podcast } from '../fetch-podcasts/fetch-podcast.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-fetch-episodes',
  templateUrl: './fetch-episodes.component.html'
})
export class FetchEpisodesComponent {
  public episodes: Episodes[];
  public podcastTitle: string;
  public podcastId: number;
  private httpContext: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, route: ActivatedRoute, @Inject('BASE_URL') baseUrl: string) {
    this.httpContext = http;
    this.baseUrl = baseUrl;
    
    if (route.snapshot.queryParams['podcast']) {
      let podcastId = route.snapshot.queryParams['podcast'];
      this.httpContext.get<Podcast>(this.baseUrl + 'api/podcasts/' + podcastId).subscribe(
        result => {
          this.podcastTitle = result.title;
          this.podcastId = result.id;

          this.readEpisodes(this.podcastId);

        },
        error => {
          console.error(error)
        });
    } else {
      this.podcastTitle = "There is no active podcast selected.";
    }

    this.readEpisodes(0);
  }

  // CRUD - read
  public readEpisodes(podcastId: number) {
    if (podcastId > 0) {
      this.httpContext.get<Episodes[]>(this.baseUrl + 'api/episodes/podcastepisodes/' + podcastId).subscribe(result => {
        this.episodes = result;
      }, error => console.error(error));
    } else {
      this.episodes = [];
    }
  }

  // CRUD - delete
  removeEpisode(inputId: number) {
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
      var newEpisode = {
        id: 0,
        podcastid: inputpodcastid,
        title: episode.title,
        description: episode.content,
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
