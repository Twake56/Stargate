import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { PersonDutyResponse } from './app-interfaces';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private httpClient:HttpClient) { }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      //'Access-Control-Allow-Origin': 'https://localhost:62088'
    })
  }

  addPerson(name: string) {
    let url = "https://localhost:7071/Person";
    //return this.httpClient.get(url);
    return this.httpClient.post(url, JSON.stringify(name), this.httpOptions);
  }

  getPeople() {
    let url = "https://localhost:7071/Person";
    return this.httpClient.get(url);
  }

  getAstronautDuties(name: string) {
    let url = "https://localhost:7071/AstronautDuty/"+ name;
    return this.httpClient.get<PersonDutyResponse>(url);
  }
}
