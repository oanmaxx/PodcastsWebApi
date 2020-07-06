import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { FetchPodcastComponent } from './fetch-podcasts/fetch-podcast.component';
import { FetchEpisodesComponent } from './fetch-episodes/fetch-episodes.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TextInputDialogComponent } from './text-input-dialog/text-input-dialog.component';
import { MatButtonModule, MatFormFieldModule, MatInputModule, MatDialogModule, MatCheckboxModule } from '@angular/material';
import { ConfirmationDialog } from './confirm-dialog/confirmation-dialog';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    FetchPodcastComponent,
    FetchEpisodesComponent,
    TextInputDialogComponent,
    ConfirmationDialog
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'fetch-podcast', component: FetchPodcastComponent },
      { path: 'fetch-episodes', component: FetchEpisodesComponent },
    ]),
    BrowserAnimationsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatCheckboxModule
  ],
  entryComponents: [
    TextInputDialogComponent,
    ConfirmationDialog
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
