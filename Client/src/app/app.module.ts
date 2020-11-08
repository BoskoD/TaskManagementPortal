import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProjectsComponent } from './projects/projects.component';
import { ShowProjectsComponent } from './projects/show-projects/show-projects.component';
import { AddUpdateProjectComponent } from './projects/add-update-project/add-update-project.component';
import { TasksComponent } from './tasks/tasks.component';
import { ShowTasksComponent } from './tasks/show-tasks/show-tasks.component';
import { AddUpdateTaskComponent } from './tasks/add-update-task/add-update-task.component';
import {SharedService} from './shared.service';

import {HttpClientModule} from '@angular/common/http'
import {FormsModule,ReactiveFormsModule} from '@angular/forms';


@NgModule({
  declarations: [
    AppComponent,
    ProjectsComponent,
    ShowProjectsComponent,
    AddUpdateProjectComponent,
    TasksComponent,
    ShowTasksComponent,
    AddUpdateTaskComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [SharedService],
  bootstrap: [AppComponent]
})
export class AppModule { }
