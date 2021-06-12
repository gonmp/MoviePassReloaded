import IGenre from './genre';

enum MovieFields {
    TITLE = "title",
    LANGUAGE = "language",
    IMAGE = "image",
    OVERVIEW = "overview",
    DURATION = "duration",
    GENRES = "genres"
}

export default interface IMovie {
    id?: number,
    [MovieFields.TITLE]: string,
    [MovieFields.LANGUAGE]: string,
    [MovieFields.IMAGE]: string,
    [MovieFields.OVERVIEW]: string,
    [MovieFields.DURATION]: number,
    [MovieFields.GENRES]: IGenre[]
}