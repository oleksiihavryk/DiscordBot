import { Component } from '@angular/core';
import { BotService } from 'src/app/services/bot.service';

@Component({
  selector: 'app-bot-switch-button',
  templateUrl: './bot-switch-button.component.html',
  styleUrls: ['./bot-switch-button.component.css']
})
export class BotSwitchButtonComponent {
  constructor(public botService: BotService) {}
}
