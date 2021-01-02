import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatToolbarModule } from "@angular/material/toolbar";
import { FlexLayoutModule } from "@angular/flex-layout";
import { HomeComponent } from './home/home/home.component';
import { AddUpdateProjectComponent } from './projects/add-update-project/add-update-project.component';
import { AddUpdateTaskComponent } from './tasks/add-update-task/add-update-task.component';
import { ShowProjectsComponent } from './projects/show-projects/show-projects.component';
import { ShowTasksComponent } from './tasks/show-tasks/show-tasks.component';
import { SharedService } from '../app/shared.service'
import { ProjectsComponent } from './projects/projects.component';
import { TasksComponent } from './tasks/tasks.component';

export function tokenGetter(){
  return localStorage.getItem('jwt')
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    AddUpdateProjectComponent,
    AddUpdateTaskComponent,
    ShowProjectsComponent,
    ShowTasksComponent,
    ProjectsComponent,
    TasksComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatToolbarModule,
    FlexLayoutModule
  ],
  providers: [SharedService],
  bootstrap: [AppComponent]
})
export class AppModule { }
