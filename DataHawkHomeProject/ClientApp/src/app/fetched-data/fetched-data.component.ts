import { Component, Inject, OnChanges, SimpleChanges, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetched-data',
  templateUrl: './fetched-data.component.html'
})
export class FetchedDataComponent {
  public reviews: Reviews[];
  public allReviews: Reviews[];
  private reverseOrder: boolean = false;
  private sortedColumn: string;
  @Input('searchText') searchText: string = '';

  searchItems(searchText: string)
  {
    console.log("Search input : " + searchText)
    this.reviews = this.allReviews.filter(r => r.asin.toLowerCase().includes(searchText.toLowerCase()) || r.reviewText.toLowerCase().includes(searchText.toLowerCase()));
  }

  sortColumn(columnToSort: string) {
    if (this.sortedColumn == columnToSort)
      this.reverseOrder = !this.reverseOrder;
    else {
      this.reverseOrder = false;
      this.sortedColumn = columnToSort;
    }

    if (columnToSort == "asin") {
      if (!this.reverseOrder)
        this.reviews = this.reviews.sort((a, b) => a.asin > b.asin ? 1 : -1);
      else
        this.reviews = this.reviews.sort((a, b) => a.asin > b.asin ? 1 : -1).reverse();
    }
    else if (columnToSort == "review") {
      if (!this.reverseOrder)
        this.reviews = this.reviews.sort((a, b) => a.reviewText > b.reviewText ? 1 : -1);
      else
        this.reviews = this.reviews.sort((a, b) => a.reviewText > b.reviewText ? 1 : -1).reverse();
    }
    else if (columnToSort == "rating") {
      if (!this.reverseOrder)
        this.reviews = this.reviews.sort((a, b) => a.rating > b.rating ? 1 : -1);
      else
        this.reviews = this.reviews.sort((a, b) => a.rating > b.rating ? 1 : -1).reverse();
    }
    else if (columnToSort == "title") {
      if (!this.reverseOrder)
        this.reviews = this.reviews.sort((a, b) => a.title > b.title ? 1 : -1);
      else
        this.reviews = this.reviews.sort((a, b) => a.title > b.title ? 1 : -1).reverse();
    }
    else if (columnToSort == "date") {
      if (!this.reverseOrder)
        this.reviews = this.reviews.sort((a, b) => a.date > b.date ? 1 : -1);
      else
        this.reviews = this.reviews.sort((a, b) => a.date > b.date ? 1 : -1).reverse();
    }

  }

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Reviews[]>(baseUrl + 'api/asin').subscribe(result => {
      this.reviews = result;
      this.allReviews = this.reviews;
      console.log(this.reviews);
    }, error => console.error(error));
  }
}

interface Reviews {
  asin: string;
  reviewText: string;
  rating: string;
  title: string;
  date: string;
}
