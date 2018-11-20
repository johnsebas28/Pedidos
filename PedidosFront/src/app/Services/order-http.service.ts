import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {Observable } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class OrderHttpService {

  constructor(private httpClient:HttpClient) { }

  get(url: string,headers:HttpHeaders): Observable<any>{
    return this.httpClient.get<any>(url,{headers:headers});
  }

  post(url: string,data:any,headers:HttpHeaders): Observable<any>{
    return this.httpClient.post<any>(url, data ,{headers:headers});
  }

  put(url: string,data:any,headers:HttpHeaders): Observable<any>{
    return this.httpClient.put<any>(url, data ,{headers:headers});
  }
}
