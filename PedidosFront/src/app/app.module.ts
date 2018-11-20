import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { LoginComponent } from './Components/login/login.component';
import { HighligthDirective } from './Directives/highligth.directive';
import { UserComponent } from './Components/user/user.component';
import { FileUploadComponent } from './Components/file-upload/file-upload.component';
import { OrderHttpService } from './Services/order-http.service';
import { NotificationsService } from './Services/notifications.service';
import { HttpClientModule } from '@angular/common/http';
import { ModalUserComponent } from './Components/user/modal/modal-user/modal-user.component';
//Animatios
import { BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { MaterialModule} from './material.module';
import { NotificationAlertComponent } from './Components/Common/notification-alert/notification-alert.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HighligthDirective,
    UserComponent,
    FileUploadComponent,
    ModalUserComponent,
    NotificationAlertComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
    ReactiveFormsModule
  ],
  providers: [
    OrderHttpService,
    NotificationsService],
  bootstrap: [AppComponent],
  entryComponents:[
    ModalUserComponent,
    NotificationAlertComponent
  ]
})
export class AppModule { }
