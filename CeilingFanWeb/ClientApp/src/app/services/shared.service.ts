import { throwError, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';

//Import for Http access REST endpoints
import { HttpHeaders, HttpErrorResponse } from '@angular/common/http';


//@Injectable({
//  providedIn: 'root'
//})

/**
* Reference for the environment
*/
//export const API_URI = environment.apiUrl;
export const API_URI = document.getElementsByTagName('base')[0].href;

export abstract class SharedService {


  /**
   * Set up for our Http options for REST API Comms
   */
  protected httpOptions(): object {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',//The content type we are sending
        'Accept': 'application/json'//API  return a JSON
      })
    };
    return httpOptions;
  }

  /**
   * Http error handler
   * @param err the HttpErrorResponse from the REST API
   */
  protected handleError(err: HttpErrorResponse): Observable<never> {
    if (err.error instanceof ProgressEvent) return throwError(['Error connecting to REST API']);

    return throwError(err.error);
  };
}
