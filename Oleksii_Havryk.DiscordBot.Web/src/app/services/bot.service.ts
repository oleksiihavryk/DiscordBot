import { Injectable } from '@angular/core';
import { from, map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BotService {
  private onRequest: boolean = false;
  private botIsWorking: boolean = false;

  public get isOn(): boolean {
    return this.botIsWorking;
  }
  public get isOff(): boolean {
    return !this.botIsWorking;
  }

  constructor() { 
    //Try to get status of bot
    const sub = from(fetch('https://localhost:10001/bot', {
      headers: {
        'Content-Type': 'application/json'
      },
      method: 'GET'
    })).subscribe({
      next: async (value) => {
        if (value.ok) {
          const res = await value.json();
          this.botIsWorking = res.enabled;
        } else {
          throw new Error('Occurred unknown error from checking bot status.');
        }
      },
      complete: () => {
        console.log('Request is completed!');
        this.botIsWorking = false;
        this.onRequest = false;
        sub.unsubscribe();
      },
      error: (err) => {
        console.error(err);
        this.onRequest = false;
        sub.unsubscribe();
      }});
  }

  public offBot() {
    if (this.onRequest || this.isOff) return;

    this.onRequest = true;

    const requestToOff = this.getOffBotRequest();
    const sub = requestToOff.subscribe({
      next: (value) => {
        if (value.ok) {
          console.log('Bot is disabled successfully!')
        } else {
          throw new Error('Unknown error from bot while disabling.');
        }
      },
      complete: () => {
        console.log('Request is completed!');
        this.botIsWorking = false;
        this.onRequest = false;
        sub.unsubscribe();
      },
      error: (err) => {
        console.error(err);
        this.onRequest = false;
        sub.unsubscribe();
      }
    })
  }
  public onBot() {
    if (this.onRequest || this.isOn) return;

    this.onRequest = true;

    const requestToOn = this.getOnBotRequest();
    const sub = requestToOn.subscribe({
      next: (value) => {
        if (value.ok) {
          console.log('Bot is enabled successfully!')
        } else {
          throw new Error('Unknown error from bot while enabling.');
        }
      },
      complete: () => {
        console.log('Request is completed!');
        this.botIsWorking = true;
        this.onRequest = false;
        sub.unsubscribe();
      },
      error: (err) => {
        console.error(err);
        this.onRequest = false;
        sub.unsubscribe();
      }
    })
  }
  private getOnBotRequest(): Observable<Response> {
    return from(fetch('https://localhost:10001/bot/run', {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      }
    }));
  }
  private getOffBotRequest(): Observable<Response> {
    return from(fetch('https://localhost:10001/bot/stop', {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      }
    }));
  }
}
