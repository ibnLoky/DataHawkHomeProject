import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormControl } from '@angular/forms';

export class AsinDto {
  public asins: string;
}

export class Asin {
  public status: Number;
  public statusDisplay: string;
  public asinNumber: string; 
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent {

  public asins: Asin[];
  private readonly _httpClient: HttpClient;
  private readonly _baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._httpClient = http;
    this._baseUrl = baseUrl;
    console.log("baseurl : " + baseUrl);
    this.asins = new Array<Asin>();
    http.get<Asin[]>(baseUrl + 'api/asin/loadedAsinsList').subscribe(result => {
      this.asins = result;
      console.log(this.asins);
    }, error => console.error(error));

  }

  public addItems(text: string) {
    var asins = text.split(',');
    asins.forEach(asin => this.addItem(asin));
  }

  public addItem = function (text: string) {
    var dto: AsinDto = { asins: text };
    console.log("stringify : " + JSON.stringify(dto))
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    if (!this.asins.some(a => a.asinNumber == text && a.statusDisplay == "Done")) {
      var asin: Asin = {
        asinNumber: text,
        status: 1,
        statusDisplay: "In progress"
      }
      this.asins.push(asin);

      var response = this._httpClient.post(this._baseUrl + "api/asin", JSON.stringify(dto), httpOptions).subscribe(
        () => asin.statusDisplay = "Done",
        () => asin.statusDisplay = "Error");
      console.log("Response : " + response);
    }
    else (console.log("Asin already added : " + text))
  };
}
