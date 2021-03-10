import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'My cool dating app';
  users: any;
  //inject the http component in constructor
  constructor(private http: HttpClient){}

  //This is an angular lifecycle method that fires after the constructor
  ngOnInit(): void {
    this.getUsers();
  }

  getUsers()
  {
    this.http.get("https://localhost:5001/api/users").subscribe(response =>
      {
        this.users = response;
        console.log(response);
      }, 
      error =>
      {
        console.log(error);
      })
  }
}
