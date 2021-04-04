import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Fan } from '../models/fan';
import { FanService } from '../services/fan.service';
import { ActivatedRoute } from '@angular/router';  

@Component({
  selector: 'app-fan',
  templateUrl: './fan.component.html',
  styleUrls: ['./fan.component.css']
})
export class FanComponent implements OnInit {

  /**
   * Currency
   */
  public fan: Fan;

  public isValidFormSubmitted: boolean = false;

  public loadingFan: boolean = false;

  public fanSaving: boolean = false;


  public imgSpeed0_PullCord: boolean = true;
  public imgSpeed0_Cord: boolean = false;

  public imgSpeed1_PullCord: boolean = false;
  public imgSpeed1_Cord: boolean = false;

  public imgSpeed2_PullCord: boolean = false;
  public imgSpeed2_Cord: boolean = false;

  public imgSpeed3_PullCord: boolean = false;
  public imgSpeed3_Cord: boolean = false;

  public imgDirectionForward_PullCord: boolean = true;
  public imgDirectionForward_Cord: boolean = false;
  public imgDirectionReverse_PullCord: boolean = false;
  /**
   * Save Message
   */
  public saveMessage: string;

  /**
   * Error messages
   */
  public messages: string[] = [];

  /**
   * 
   * @param currencyService 
   */
  constructor(private fanService: FanService, private route: ActivatedRoute) {
  }


  /**
   * ngOnInit
   */
  ngOnInit() {
    console.log(document.getElementsByTagName('base')[0].href);
    this.fan = new Fan();
    this.fan.FanId = 0;

    if (this.route.snapshot.paramMap.get("id") != null) {
      let id: number = parseInt(this.route.snapshot.paramMap.get("id"));
      this.fanService.getFan(id).subscribe(
        fan => {
          this.fan = fan;
          this.processFan();
        }, err => {
          this.fan = new Fan();
          this.fan.FanId = 0;
        }).add(() => { this.fanSaving = false; });
    }
    
  }

  processFan() {
    //Update Speed Pull Cord

    if (this.fan.Speed == 0) {
      this.imgSpeed0_PullCord = true;
      this.imgSpeed0_Cord = false;
      this.imgSpeed1_PullCord = false;
      this.imgSpeed1_Cord = false;
      this.imgSpeed2_PullCord = false;
      this.imgSpeed2_Cord = false;
      this.imgSpeed3_PullCord = false;
    }

    if (this.fan.Speed == 1) {
      this.imgSpeed0_PullCord = false;
      this.imgSpeed0_Cord = true;

      this.imgSpeed1_PullCord = true;
      this.imgSpeed1_Cord = false;
    }

    if (this.fan.Speed == 2) {
      this.imgSpeed0_PullCord = false;
      this.imgSpeed0_Cord = true;
      this.imgSpeed1_PullCord = false;
      this.imgSpeed1_Cord = true;

      this.imgSpeed2_PullCord = true;
      this.imgSpeed2_Cord = false;
    }

    if (this.fan.Speed == 3) {

      this.imgSpeed0_PullCord = false;
      this.imgSpeed0_Cord = true;
      this.imgSpeed1_PullCord = false;
      this.imgSpeed1_Cord = true;

      this.imgSpeed2_PullCord = false;
      this.imgSpeed2_Cord = true;

      this.imgSpeed3_PullCord = true;
    }

    //Update Speed Direction Cord
    if (this.fan.Direction == 1) {
      this.imgDirectionForward_PullCord = false;
      this.imgDirectionForward_Cord = true;

      this.imgDirectionReverse_PullCord = true;
    }
    else {
      this.imgDirectionForward_PullCord = true;
      this.imgDirectionForward_Cord = false;

      this.imgDirectionReverse_PullCord = false;
    }
  }


  onPullSpeedCordClick() {
    console.log("Speed Cord")
    let updFan = new Fan();
    updFan.FanId = this.fan.FanId;
    updFan.Description = this.fan.Description;
    updFan.Speed = this.fan.Speed == 3 ? 0 : this.fan.Speed + 1;
    updFan.Direction = this.fan.Direction;
    updFan.CreatedAt = this.fan.CreatedAt;

    this.updateFan(updFan);
  }

  onPullDirectionCordClick() {
    console.log("Direction Cord");
    let updFan = new Fan();
    updFan.FanId = this.fan.FanId;
    updFan.Description = this.fan.Description;
    updFan.Speed = this.fan.Speed ;
    updFan.Direction = this.fan.Direction == 1 ? 0 : 1;
    updFan.CreatedAt = this.fan.CreatedAt;

    this.updateFan(updFan);
  }

  updateFan(updateFan : Fan) {
    //Call createFan method from currency service
    //Consuming REST api
    this.fanSaving = true;
    this.fanService.updateFan(updateFan).subscribe(
      fan => {
        if (fan.FanId != 0) {
          this.fan = fan;
          this.processFan();
        } else {
          this.isValidFormSubmitted = false;
          this.messages.push('An error occured creating the fan via REST API');
        }
      }, err => {
        this.messages.push(...err);
      }).add(() => { this.fanSaving = false; });
  }


  /**
   * Form Submission event handler
   */
  onFormSubmit(form: NgForm) {
    this.isValidFormSubmitted = false;

    if (form.invalid) {
      return;
    }

    this.fanSaving = true;
    this.fan = form.value;
    this.messages = [];
    this.saveMessage = '';

    //Call createFan method from currency service
    //Consuming REST api
    this.fanService.createFan(this.fan).subscribe(
      fan => {
        if (fan.FanId != 0) {
          this.saveMessage = `Fan was created. Id: ${fan.FanId}`;
          this.isValidFormSubmitted = true;
          form.resetForm();
          this.fan = fan;
          this.processFan();
        } else {
          this.isValidFormSubmitted = false;
          this.messages.push('An error occured creating the fan via REST API');
        }
      }, err => {
        this.messages.push(...err);
      }).add(() => { this.fanSaving = false; });

  }


  /**
   * Form Submission event handler
   */
  onFormUpdateSubmit(form: NgForm) {
    this.isValidFormSubmitted = false;

    if (form.invalid) {
      return;
    }

    this.fanSaving = true;
    this.fan = form.value;
    this.messages = [];
    this.saveMessage = '';

    //Call createFan method from currency service
    //Consuming REST api
    this.fanService.createFan(this.fan).subscribe(
      fan => {
        console.warn(fan.FanId);
        if (fan.FanId != 0) {
          this.saveMessage = `Fan was created. Id: ${fan.FanId}`;
          this.isValidFormSubmitted = true;
          form.resetForm();
        } else {
          this.isValidFormSubmitted = false;
          this.messages.push('An error occured creating the fan via REST API');
        }
      }, err => {
        this.messages.push(...err);
      }).add(() => { this.fanSaving = false; });

  }
}
