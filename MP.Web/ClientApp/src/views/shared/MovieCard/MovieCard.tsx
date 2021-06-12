import { Card, CardContent, CardHeader, CardMedia, makeStyles, Typography } from '@material-ui/core';
import * as React from 'react';
import IMovie from '../../../models/movie';
import { imagesUrl } from '../../../constants/theMovieDbUrls';

export interface IMovieCardProps {
    movie: IMovie
}

const useStyles = makeStyles({
    media: {
        height: 140
    }
});

const MovieCard = ({ movie }: IMovieCardProps) => {
    const classes = useStyles();
    return (
        <Card>
            <CardMedia
                className={classes.media}
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