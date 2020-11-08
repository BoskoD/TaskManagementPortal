import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {ProjectsComponent} from './projects/projects.component';
import {TasksComponent} from './tasks/tasks.component';

const routes: Routes = [
  {path:'projects',component:ProjectsComponent},
  {path:'tasks',component:TasksComponent}
  
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
