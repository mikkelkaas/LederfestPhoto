import { TeamServiceService } from './team-service.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { TeamComponent } from './team/team.component';
import { RequestOptionsService } from './RequestOptionsService';
import { RequestOptions } from '@angular/http';
import { Ng2ImgMaxModule } from 'ng2-img-max';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';

import { ChallengeComponent } from './challenge/challenge.component';
import { DoneComponent } from './done/done.component';
import { RateComponent } from './rate/rate.component';
import { ShowComponent } from './show/show.component';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';

const appRoutes: Routes = [
  {
    path: 'team',
    component: TeamComponent,
    data: { title: 'Lederfest Teams' }
  },
  {
    path: 'show',
    component: ShowComponent,
    data: { title: 'Lederfest Teams' }
  },
  // {
  //   path: '',
  //   component: TeamComponent,
  //   data: { title: 'Lederfest Teams' }
  // },
  {
    path: 'challenge',
    component: ChallengeComponent,
    data: { title: 'Billeder' }
  },
  {
    path: 'done',
    component: DoneComponent,
    data: { title: 'Billeder' }
  },
  {
    path: 'rate',
    component: RateComponent,
    data: { title: 'Billeder' }
  },
  {
    path: '',
    redirectTo: '/team',
    pathMatch: 'full'
  },
];

@NgModule({
  declarations: [
    AppComponent,
    TeamComponent,
    ChallengeComponent,
    DoneComponent,
    RateComponent,
    ShowComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    HttpModule,
    Ng2ImgMaxModule,
    NgbModule.forRoot(),
    RouterModule.forRoot(
    appRoutes,
    { enableTracing: false, useHash: true } // <-- debugging purposes only
    )
  ],
providers: [TeamServiceService, { provide: RequestOptions, useClass: RequestOptionsService },
  { provide: LocationStrategy, useClass: HashLocationStrategy }
],
  bootstrap: [AppComponent]
})
export class AppModule { }
