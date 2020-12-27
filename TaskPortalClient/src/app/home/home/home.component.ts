import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  isUserAuthenticated() {
    const token: string = localStorage.getItem('jwt');
    if (token) {
      return true;
    } else {
      return false;
    }
  }

  logout() {
    localStorage.removeItem('jwt');
  }

}
