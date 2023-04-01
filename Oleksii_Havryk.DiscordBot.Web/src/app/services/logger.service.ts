import { Injectable } from '@angular/core';
import { map, Observable, switchMap, timer } from 'rxjs';
import { ajax } from 'rxjs/ajax';

export class LoggerMessage {
  constructor(
    public addTime: Date, 
    public source: string, 
    public message: string, 
    public isRead: boolean) {
  }
}

@Injectable({
  providedIn: 'root'
})
export class LoggerService {
  public updateIntervalInSeconds: number = 5; 
  public oldMessages: LoggerMessage[] = [];
  public newMessages: LoggerMessage[] = [];

  public get anyNewMessages(): boolean {
    return this.newMessages.length !== 0;
  }
  public get anyOldMessages(): boolean {
    return this.oldMessages.length !== 0;
  }

  private get updateIntervalInMilliseconds() {
    return this.updateIntervalInSeconds * 1000
  }

  constructor() { 
    timer(this.updateIntervalInSeconds - 1, this.updateIntervalInMilliseconds)
      .pipe(
        switchMap(() => this.requestUpdateAll())
      ).subscribe();
  }

  public requestUpdateAll(): Observable<LoggerMessage[]> {
    const requestAll = this.createUpdateRequest(null);
    const obs = requestAll.subscribe({
      next: async (value) => {
        this.newMessages = value.filter(v => v.isRead === false);
        this.oldMessages = value.filter(v => v.isRead);
      },
      complete() {
        obs.unsubscribe();
      },
      error(err) {
        console.error(err);
        obs.unsubscribe();
      }
    });

    return requestAll;
  }
  public requestUpdateOld(): Observable<LoggerMessage[]> {
    const requestOld = this.createUpdateRequest(false);
    const obs = requestOld.subscribe({
      next: (value) => {
          this.oldMessages = value;
      },
      complete() {
        obs.unsubscribe();
      },
      error(err) {
        console.error(err);
        obs.unsubscribe();
      }
    });

    return requestOld;
  }
  public requestUpdateNew(): Observable<LoggerMessage[]> {
    const requestNew = this.createUpdateRequest(true);
    const obs = requestNew.subscribe({
      next: (value) => {
         this.newMessages = value;
      },
      complete() {
        obs.unsubscribe();
      },
      error(err) {
        console.error(err);
        obs.unsubscribe();
      }
    })

    return requestNew;
  }
  
  createUpdateRequest(isNew: boolean | null = null): Observable<LoggerMessage[]> {
    let request: Observable<LoggerMessage[]> | undefined;

    switch (isNew) {
      case true: {
        request = ajax.getJSON('http://localhost:10000/logger/new');
        break;
      }
      case false: {
        request = ajax.getJSON('http://localhost:10000/logger/old');
        break;
      }
      default: {
        request = ajax.getJSON('http://localhost:10000/logger');
      }
    }

    return request.pipe(
      map((response) => {
        return response.sort((a,b) => new Date(b.addTime).getTime() - new Date(a.addTime).getTime());
      })
    )
  }
}
