import { Injectable } from '@angular/core';
import { SharedService, API_URI } from './shared.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Fan } from '../models/fan';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

/**
 * Fan Service class
 */
export class FanService extends SharedService {

  /**
   * Default Constructor
   * @param http HttpClient dependency
   */
  constructor(private http: HttpClient) { super(); }

  /**
   * Get a currency by providing the Currency Id
   * @param id the id of the Currency
   */
  public getFan(id: number): Observable<Fan> {
    const apiMethod = `${API_URI}api/Fan/${id}`;
    return this.http.get<Fan>(apiMethod).pipe(catchError(super.handleError));
  }

  /**
   * Get all fans
   */
  public getFans(): Observable<Array<Fan>> {
    const apiMethod = `${API_URI}api/Fan`;
    return this.http.get<Array<Fan>>(apiMethod).pipe(catchError(super.handleError));
  }

  /**
   * Create a new fan
   * @param currency The created fan
   */
  public createFan(fan: Fan): Observable<Fan> {
    const apiMethod = `${API_URI}api/Fan`;
    return this.http.post<Fan>(apiMethod, fan, super.httpOptions()).pipe(catchError(super.handleError));
  }

  /**
   * Update a new fan
   * @param currency The updated fan
   */
  public updateFan(fan: Fan): Observable<Fan> {
    const apiMethod = `${API_URI}api/Fan/` + fan.FanId;
    return this.http.put<Fan>(apiMethod, fan, super.httpOptions()).pipe(catchError(super.handleError));
  }
}
