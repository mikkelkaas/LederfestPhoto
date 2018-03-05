import { TeamServiceService } from './../team-service.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})
export class TeamComponent implements OnInit {

  teams: any;
  team: any;
  constructor(private teamServiceService: TeamServiceService,
    private router: Router
  ) { }

  ngOnInit() {
    this.team = JSON.parse(localStorage.getItem('team'));
    if (this.team != null) {
      console.log('GOT TEAM', this.team);
    }
    this.teamServiceService.getTeams().subscribe(teams => {
      this.teams = teams;
      console.log(this.teams);
    });
  }

  selectTeam(id: string) {
    console.log('Selected team', id);
    const team = this.teams.find(t => t.id === id);
    console.log('Got team', team);
    localStorage.setItem('team', JSON.stringify(team));
    this.router.navigateByUrl('challenge');

  }
  createTeam(teamname: string) {
    this.teamServiceService.addTeam(teamname).subscribe(team => {
      console.log('Created team with id', team.id);
      this.team = team;
      localStorage.setItem('teamid', team.id);
      localStorage.setItem('teamname', team.name);
      localStorage.setItem('team', JSON.stringify(team));
      this.router.navigateByUrl('challenge');

    });
  }

}
