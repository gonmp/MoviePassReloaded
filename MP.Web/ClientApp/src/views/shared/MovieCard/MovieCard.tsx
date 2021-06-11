import { Card, CardContent, CardHeader, CardMedia, Typography } from '@material-ui/core';
import * as React from 'react';
import IMovie from '../../../models/movie';
import { imagesUrl } from '../../../constants/theMovieDbUrls';

export interface IMovieCardProps {
    movie: IMovie
}

const MovieCard = ({ movie }: IMovieCardProps) => {
    return (
        <Card>
            <CardMedia
                image={`${imagesUrl}${movie.image}`}
            />
            <CardContent>
                <Typography>
                    {movie.title}
                </Typography>
                <Typography>
                    {movie.overview}
                </Typography>
            </CardContent>
        </Card>
    );
}

export default MovieCard;