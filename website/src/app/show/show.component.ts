import { Observable } from 'rxjs/Observable';
import { TeamServiceService } from './../team-service.service';
import { Component, OnInit } from '@angular/core';
import { TimerObservable, } from 'rxjs/observable/TimerObservable';
import { Subscription, } from 'rxjs/Subscription';
import 'rxjs/add/observable/timer';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-show',
  templateUrl: './show.component.html',
  styleUrls: ['./show.component.scss'],
  providers: [NgbCarouselConfig]
})
export class ShowComponent implements OnInit {
  private timer;
  private ticks = 0;
  private sub: Subscription;
  photos: any;
  constructor(private teamServiceService: TeamServiceService,
    config: NgbCarouselConfig) {
    config.interval = 4000;

  }

  ngOnInit() {
    // this.timer = Observable.timer(2000, 10000);
    // subscribing to a observable returns a subscription object
    // this.sub = this.timer.subscribe(t => this.tickerFunc(t));
    this.getAll();
  }

  getAll() {
    this.teamServiceService.getAllPhotos().subscribe(photos => {
      this.photos = photos;
    });

  }

}
