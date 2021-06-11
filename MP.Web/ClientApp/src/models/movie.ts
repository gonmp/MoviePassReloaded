enum MovieFields {
    TITLE = "title",
    LANGUAGE = "language",
    IMAGE = "image",
    OVERVIEW = "overview",
    DURATION = "duration"
}

export default interface IMovie {
    id?: number,
    [MovieFields.TITLE]: string,
    [MovieFields.LANGUAGE]: string,
    [MovieFields.IMAGE]: string,
    [MovieFields.OVERVIEW]: string,
    [MovieFields.DURATION]: number
}