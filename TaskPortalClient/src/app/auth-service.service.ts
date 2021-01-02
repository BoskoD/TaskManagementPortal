import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { baseUrl } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {

  constructor(private http: HttpClient) { }
    
  login(data): Observable<any>{
    console.log('Hello from the server');
    return this.http.post(`${baseUrl}account/login`, data);
  }
}
