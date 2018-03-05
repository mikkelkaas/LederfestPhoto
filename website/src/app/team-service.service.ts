import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import { Observable } from 'rxjs/Observable';
import { AppSettings } from '../app/app.settings';

@Injectable()
export class TeamServiceService {

  getPhotoForRating(): any {
    return this.http.get(AppSettings.API_ENDPOINT + 'Photos/GetUnrated')
      .map(res => res.json());
  }
  constructor(private http: Http) { }
  getTeams() {
    console.log('Getting teams');
    return this.http.get(AppSettings.API_ENDPOINT + 'Teams')
      .map(res => res.json());
  }
  getAllPhotos() {
    return this.http.get(AppSettings.API_ENDPOINT + 'Photos/GetAll')
      .map(res => res.json());
  }

  addTeam(name: string) {
    return this.http.post(AppSettings.API_ENDPOINT + 'Teams', JSON.stringify({ name: name }))
      .map(res => res.json());
  }

  rate(photo: string, rating: number) {
    return this.http.put(AppSettings.API_ENDPOINT + 'Photos/' + photo + '/', JSON.stringify({ rating: rating }));
  }


  getChallenges() {
    return this.http.get(AppSettings.API_ENDPOINT + 'Challenges')
      .map(res => res.json());
  }


  getChallengesForTeam(arg0: any): any {
    console.log('get challenges for team', arg0);
    return this.http.get(AppSettings.API_ENDPOINT + 'Challenges/' + arg0)
      .map(res => res.json());
  }
}
