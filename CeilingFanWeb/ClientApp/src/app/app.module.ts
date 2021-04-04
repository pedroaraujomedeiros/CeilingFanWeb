import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { FanListComponent } from './fan-list/fan-list.component';
import { FanComponent } from './fan/fan.component';
import { FanService } from './services/fan.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FanListComponent,
    FanComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      //{ path: '', component: HomeComponent, pathMatch: 'full' },
      { path: '', redirectTo: '/fan', pathMatch: 'full' },
      //{ path: 'counter', component: CounterComponent },
      //{ path: 'fetch-data', component: FetchDataComponent },
      { path: 'fan', component: FanComponent },
      { path: 'fan/:id', component: FanComponent },
      { path: 'fans', component: FanListComponent },

    ])
  ],
  providers: [FanService],
  bootstrap: [AppComponent]
})
export class AppModule { }
