export interface MovieSearchResponse {
    title: string;
    year : string;
    genre: string;
    poster: string;
    plot: string;
    released: string;
}

export interface MoviePublishRequest {
    name: string;
    description: string;
    genre: string;
    releaseDate: Date;
    posterUri: string;
}

export interface MoviePublishResponse {
    id: string;
    name: string;
    description: string;
    genre: string;
    releaseDate: Date;
    posterUri: string;
}