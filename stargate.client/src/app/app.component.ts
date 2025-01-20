import { HttpEventType } from '@angular/common/http';
import { Component } from '@angular/core';
import { ApiService } from './api.service';
import { AstronautDuty, Person } from './app-interfaces';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  astronautDuties: AstronautDuty[] = [];

  astronautPerson: Partial<Person> = {};

  astronautName = '';

  progress: number = 0;

  errorMessage: string = '';

  constructor(private apiService: ApiService) { }

  getAstronautDuties(name: string) {
    this.apiService.getAstronautDuties(name).subscribe({
      next: (event) => {
        if (event.type === HttpEventType.DownloadProgress) {
          if (event.total) {
            this.progress = Math.round(100 * event.loaded / event.total);
          }
        }
        else if (event.type === HttpEventType.Response) {
          if (!event.body.success) {
            this.errorMessage = event.body.message;
            this.astronautPerson.name = '';
          }
          else {
            this.errorMessage = '';
            this.astronautDuties = event.body.astronautDuties;
            this.astronautPerson = event.body.person;
          }
          this.progress = 100;
        }
      },
      error: (err) => {
        console.log(err);
        this.progress = 0;
        this.errorMessage = 'Error occurred getting astronaut duties'
      },
      complete: () => { console.log("Success getting astronaut duties"); }
    });
  }
}
