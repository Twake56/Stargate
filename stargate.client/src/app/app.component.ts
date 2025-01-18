import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ApiService } from './api.service';
import { AstronautDuty, Person, PersonDutyResponse } from './app-interfaces';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  personName = '';

  people = [];

  astronautDuties: AstronautDuty[] = [];

  astronautPerson: Partial<Person> = {};

  astronautName = '';


  constructor(private apiService: ApiService) { }

  addPerson() {
    this.apiService.addPerson(this.personName).subscribe({
      error: (err) => { console.log(err) },
      complete: () => { console.log("Success adding person") }
    });
  }

  getPeople() {
    this.apiService.getPeople().subscribe({
      error: (err) => { console.log(err) },
      complete: () => { console.log("Success getting people") }
    });
  }

  getAstronautDuties(name: string) {
    this.apiService.getAstronautDuties(name).subscribe( {
      next: (response) => {
        this.astronautDuties = response.astronautDuties;
        this.astronautPerson = response.person;
      },
      error: (err) => { console.log(err) },
      complete: () => { console.log("Success getting people") }
    });
  }
  
}
