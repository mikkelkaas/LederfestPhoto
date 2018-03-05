import { Injectable } from '@angular/core';
import { BaseRequestOptions } from '@angular/http';

@Injectable()
export class RequestOptionsService extends BaseRequestOptions {
  constructor() {
    super();
    this.headers.set('Content-Type', 'application/json');
  }
}
