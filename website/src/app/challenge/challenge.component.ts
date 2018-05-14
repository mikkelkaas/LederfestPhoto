import { HttpHeaders, HttpRequest, HttpEventType, HttpClient, HttpResponse } from '@angular/common/http';
import { TeamServiceService } from './../team-service.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { RequestOptions, Http, Headers } from '@angular/http';
import { AppSettings } from '../app.settings';
import { Router } from '@angular/router';
import { Ng2ImgMaxService } from 'ng2-img-max';
@Component({
  selector: 'app-challenge',
  templateUrl: './challenge.component.html',
  styleUrls: ['./challenge.component.scss']
})
export class ChallengeComponent implements OnInit {
  @ViewChild('fileholder') fileInputVariable: any;
  team: any;
  fileholder: any;
  disable = false;
  challenges: any;
  text = 'Indsend billede';
  selectedChallenge: string;
  constructor(
    private teamServiceService: TeamServiceService,
    private http: Http,
    private httpClient: HttpClient,
    private router: Router,
    private ng2ImgMax: Ng2ImgMaxService
  ) { }

  ngOnInit() {
    this.team = JSON.parse(localStorage.getItem('team'));
    this.teamServiceService.getChallengesForTeam(this.team.id).subscribe(challenges => {
      if (challenges.length === 0) {
        this.router.navigateByUrl('done');
      }
      this.challenges = challenges;
      this.selectedChallenge = this.challenges[0].id;
      console.log(this.challenges);
    });
  }
  fileChange(challenge) {
    console.log(challenge);
    this.text = 'Uploader';
    this.disable = true;
    const fileList = this.fileInputVariable.nativeElement.files;
    if (fileList.length > 0) {
      const file: File = fileList[0];
      this.ng2ImgMax.resizeImage(file, 10000, 1000).subscribe(
        result => {
          console.log('Resized image');
          const uploadedImage = new File([result], result.name);
          const formData: FormData = new FormData();
          formData.append('photo', uploadedImage, file.name);
          formData.append('Team', this.team.id);
          // formData.append('Text', 'TestText');
          formData.append('Challenge', challenge);
          const headerss = new Headers();
          /** No need to include Content-Type in Angular 4 */
          // headerss.append('Content-Type', 'multipart/form-data');
          headerss.append('Accept', 'application/json');
          const options = new RequestOptions({ headers: headerss });


          this.http.post(AppSettings.API_ENDPOINT + 'Photos/', formData, options)
            .subscribe(
              data => {
                console.log('uploaded');
                window.location.reload();
              },
              error => console.log(error)
            );
        },
        error => {
          console.log('😢 Oh no!', error);
        }
      );

    }
  }
  fileChangeNew(challenge) {
    console.log(challenge);
    this.text = 'Uploader';
    const httpOptions = {
      reportProgress: true

      // headers: new HttpHeaders({
      //   'Content-Type': 'application/json'
      // })
    };
    this.disable = true;
    const fileList = this.fileInputVariable.nativeElement.files;
    if (fileList.length > 0) {
      const file: File = fileList[0];
      this.text = 'Resizer';

      this.ng2ImgMax.resizeImage(file, 10000, 1000).subscribe(
        result => {
          this.text = 'Uploader';

          console.log('Resized image');
          const uploadedImage = new File([result], result.name);
          const formData: FormData = new FormData();
          formData.append('photo', uploadedImage, file.name);
          formData.append('Team', this.team.id);
          // formData.append('Text', 'TestText');
          formData.append('Challenge', challenge);
          console.log('Challenge', challenge);
          console.log('formData', formData);
          // const req = this.httpClient.post(AppSettings.API_ENDPOINT + 'Photos/', formData, httpOptions);
          const req = new HttpRequest('POST', AppSettings.API_ENDPOINT + 'Photos/', formData, httpOptions);
          this.httpClient.request(req).subscribe(event => {
            // Via this API, you get access to the raw event stream.
            // Look for upload progress events.
            if (event.type === HttpEventType.UploadProgress) {
              // This is an upload progress event. Compute and show the % done:
              const percentDone = Math.round(100 * event.loaded / event.total);
              if (percentDone === 100) {
                this.text = 'Behandler';
              } else {
                this.text = 'Uploader (' + percentDone + '%)';
              }
              console.log(`File is ${percentDone}% uploaded.`);
            } else if (event instanceof HttpResponse) {
              this.text = 'Færdig';

              console.log('File is completely uploaded!');
              window.location.reload();
            }
          },
            error => console.log(error)
          );


        },
        error => {
          console.log('😢 Oh no!', error);
        }
      );

    }
  }
  selectChallenge(challenge: string) {
    this.selectedChallenge = challenge;
  }
}
