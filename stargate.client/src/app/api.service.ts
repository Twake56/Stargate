import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpEvent} from '@angular/common/http';
import { Observable } from 'rxjs';

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

  getAstronautDuties(name: string): Observable<HttpEvent<any>> {
    let url = "https://localhost:7071/AstronautDuty/"+ name;
    return this.httpClient.get<any>(url, {
      observe: 'events',
      reportProgress: true
    });
  }
}
