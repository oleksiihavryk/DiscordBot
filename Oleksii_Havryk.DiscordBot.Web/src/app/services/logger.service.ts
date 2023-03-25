import { Injectable } from '@angular/core';
import { from, mergeMap, Observable, timer } from 'rxjs';

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
        mergeMap(() => this.requestUpdateAll())
      ).subscribe();
  }

  public requestUpdateAll(): Observable<Response> {
    const requestAll = from(fetch('http://localhost:10000/logger', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json'
          }
        }));
    const obs = requestAll.subscribe({
      next: async (value) => {
        const res = value;
        if (res.ok) {
          const messages: LoggerMessage[] = await res.json();
          this.newMessages = messages
            .filter(v => v.isRead === false)
            .sort((a,b) => new Date(b.addTime).getTime() - new Date(a.addTime).getTime());
          this.oldMessages = messages
            .filter(v => v.isRead)
            .sort((a,b) => new Date(b.addTime).getTime() - new Date(a.addTime).getTime());
        } else {
          throw new Error('Unknown response from server.');
        }
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
  public requestUpdateOld(): Observable<Response> {
    const requestOld = from(fetch('http://localhost:10000/logger/old', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json'
          }
        }));
    const obs = requestOld.subscribe({
      next: async (value) => {
        const res = value;
        if (res.ok) {
          var result: LoggerMessage[] = await res.json();
          this.oldMessages = result.sort((a,b) => new Date(b.addTime).getTime() - new Date(a.addTime).getTime());;
        } else {
          throw new Error('Unknown response from server.');
        }
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
  public requestUpdateNew() : Observable<Response> {
    const requestNew = from(fetch('http://localhost:10000/logger/new', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json'
          }
        }));
    const obs = requestNew.subscribe({
      next: async (value) => {
        const res = value;
        if (res.ok) {
          var result: LoggerMessage[] = await res.json();
          this.newMessages = result.sort((a,b) => new Date(b.addTime).getTime() - new Date(a.addTime).getTime());;
        } else {
          throw new Error('Unknown response from server.');
        }
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
}
