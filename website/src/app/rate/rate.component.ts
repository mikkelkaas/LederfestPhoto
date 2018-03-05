import { Component, OnInit } from '@angular/core';
import { TeamServiceService } from '../team-service.service';

@Component({
  selector: 'app-rate',
  templateUrl: './rate.component.html',
  styleUrls: ['./rate.component.scss']
})
export class RateComponent implements OnInit {

  photo: any = {
    team: {
      name: 'loading'
    },
    challenge: {
      description: 'loading'
    }
  };
  constructor(
    private teamServiceService: TeamServiceService,

  ) { }

  ngOnInit() {
    this.teamServiceService.getPhotoForRating().subscribe(photo => {
      this.photo = photo;
    });
  }
  rate(rating: any, photo: string) {
    this.teamServiceService.rate(photo, Number(rating)).subscribe(
      success => {
        console.log(success);
        window.location.reload();
      }
    );
  }

}
