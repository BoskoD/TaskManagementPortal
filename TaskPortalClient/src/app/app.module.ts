import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';  // <<<< import it here
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AddUpdateProjectComponent } from './projects/add-update-project/add-update-project.component';
import { AddUpdateTaskComponent } from './tasks/add-update-task/add-update-task.component';
import { ShowProjectsComponent } from './projects/show-projects/show-projects.component';
import { ShowTasksComponent } from './tasks/show-tasks/show-tasks.component';
import { ProjectsComponent } from './projects/projects.component';
import { TasksComponent } from './tasks/tasks.component';
import { SharedService } from '../app/shared.service'



// used to create fake backend
import { fakeBackendProvider } from './_helpers';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { JwtInterceptor, ErrorInterceptor } from './_helpers';
import { HomeComponent } from './home';
import { AdminComponent } from './admin';
import { LoginComponent } from './login';

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        HttpClientModule,
        AppRoutingModule,
        FormsModule
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        AdminComponent,
        LoginComponent,
        AddUpdateProjectComponent,
        AddUpdateTaskComponent,
        ShowProjectsComponent,
        ShowTasksComponent,
        ProjectsComponent,
        TasksComponent
    ],
    providers: [
        [SharedService],
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },

        // provider used to create fake backend
        //fakeBackendProvider
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }