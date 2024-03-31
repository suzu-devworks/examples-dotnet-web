# Pagination

Pagination is used in some form in almost every web application to divide returned data and display it on multiple pages within one web page. Pagination also includes the logic of preparing and displaying the links to the various pages.

## References

- [Using pagination in the REST API - GitHub Docs](https://docs.github.com/ja/rest/using-the-rest-api/using-pagination-in-the-rest-api?apiVersion=2022-11-28)
- [First Steps - FastAPI Pagination](https://uriyyo-fastapi-pagination.netlify.app/)
- [REST API | GitLab](https://docs.gitlab.com/ee/api/rest/#pagination)


## Request pattern

Looking at recent methods, it seems that the following pattern is often used for pagination requests.

- limit-offset
- page-number
- cursor

### limit-offset

| parameter | description                                               |
| --------- | --------------------------------------------------------- |
| `limit`   | used to specify the number of items to fetch.             |
| `offset`  | used to specify the number of items that we need to skip. |

It is supported by many RDBMSs and can be used out-of-the-box for queries.

Response schema will contain:

- `items` - list of items paginated items.
- `limit` - number of items per page.
- `offset` - number of skipped items.
- `total` - total number of items.

### page-number

This is the most common pagination that specifies page numbers.<br>
Many UI frameworks may also be of this type.

| parameter | description                                   |
| --------- | --------------------------------------------- |
| `page`    | used to specify the page number.              |
| `size`    | used to specify the number of items per page. |

There is also the problem of duplicate results when new data is added or deleted.

Response schema will contain:

- `items` - list of items paginated items.
- `page` - current page number.
- `size` - number of items per page.
- `pages` - total number of pages.
- `total` - total number of items.


### cursor

It is similar to limit-offset pagination type, but instead of using `offset` parameter, it uses `cursor` parameter.

| parameter | description                                                          |
| --------- | -------------------------------------------------------------------- |
| `cursor`  | used to identify the position of the last item in the previous page. |

It is usually a primary key of the last item in the previous page.


## Response pattern

- Response body
- Link header
- Content-Range header
- Other pagination headers

### Response body

> abbreviated

### Link header

- [Link - HTTP | MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/Headers/Link)
- [RFC 8288 Web Linking](https://datatracker.ietf.org/doc/html/rfc8288)


### Content-Range

The RFC states:

```
 Content-Range       = byte-content-range
                         / other-content-range
```

- [Content-Range - RFC 7233](https://datatracker.ietf.org/doc/html/rfc7233#section-4.2)

As it is written as bytes or octets here, this header is thought to represent the size along with the total length of the data (Content-Length).

In other words, I don't think it's intended for pagination.


### Other pagination headers

There seem to be many sites that return custom headers like this.

| header          | description                                    |
| --------------- | ---------------------------------------------- |
| `x-next-page`   | The index of the next page.                    |
| `x-page`        | The index of the current page (starting at 1). |
| `x-per-page`    | The number of items per page.                  |
| `x-prev-page`   | The index of the previous page.                |
| `x-total`       | The total number of items.                     |
| `x-total-pages` | The total number of pages.                     |
