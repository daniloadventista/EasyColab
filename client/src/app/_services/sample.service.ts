import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Group } from '../_models/group';
import { Sample } from '../_models/sample';
import { User } from '../_models/user';
import { BusyService } from './busy.service';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})export class SampleService {

  baseUrl = environment.apiUrl;
  //TODO excluir?
    hubUrl = environment.hubUrl;
  //TODO excluir?
    private hubConnection: HubConnection;
  //TODO excluir?
    private sampleThreadSource = new BehaviorSubject<Sample[]>([]);
  //TODO excluir?
    sampleThread$ = this.sampleThreadSource.asObservable();
  //TODO excluir?
    constructor(private http: HttpClient, private busyService: BusyService) { }

  //excluir?
  createHubConnection(user: User, otherUsername: string) {
    this.busyService.busy();
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'sample?user=' + otherUsername, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build()

    this.hubConnection.start()
      .catch(error => console.log(error))
      .finally(() => this.busyService.idle());

    this.hubConnection.on('ReceiveSampleThread', samples => {
      this.sampleThreadSource.next(samples);
    })

    this.hubConnection.on('NewSample', sample => {
      this.sampleThread$.pipe(take(1)).subscribe(samples => {
        this.sampleThreadSource.next([...samples, sample])
      })
    })

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.connections.some(x => x.username === otherUsername)) {
        this.sampleThread$.pipe(take(1)).subscribe(samples => {
          samples.forEach(sample => {
            if (!sample.created) {
              sample.created = new Date(Date.now())
            }
          })
          this.sampleThreadSource.next([...samples]);
        })
      }
    })
  }
  //excluir?
  stopHubConnection() {
    if (this.hubConnection) {
      this.sampleThreadSource.next([]);
      this.hubConnection.stop();
    }
  }

  getSamples(pageNumber, pageSize, container) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResult<Sample[]>(this.baseUrl + 'samples', params, this.http);
  }

  //excluir?
  getSampleThread(username: string) {
    return this.http.get<Sample[]>(this.baseUrl + 'samples/thread/' + username);
  }
  
  async sendSample(username: string, content: string) {
    return this.hubConnection.invoke('SendSample', {recipientUsername: username, content})
      .catch(error => console.log(error));
  }

  deleteSample(id: number) {
    return this.http.delete(this.baseUrl + 'samples/' + id);
  }
}
