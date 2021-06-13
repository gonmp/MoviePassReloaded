import { Button, Card, CardContent, CardHeader, CardMedia, makeStyles, Typography } from '@material-ui/core';
import * as React from 'react';
import IMovie from '../../../models/movie';
import { imagesUrl } from '../../../constants/theMovieDbUrls';

export interface IMovieCardProps {
    movie: IMovie
}

const useStyles = makeStyles({
    card: {
        display: 'grid',
        gridTemplateColumns: '18% 82%',
        margin: '60px'
    },
    cardElement: {
        marginTop: '20px',
        marginBottom: '20px'
    },
    media: {
        display:'inline-block',
        height: '300px',
        objectFit: 'contain',
        paddingTop: '20px',
        paddingBottom: '20px'
    }
});

const MovieCard = ({ movie }: IMovieCardProps) => {
    const classes = useStyles();
    return (
        <Card className={classes.card}>
            <CardMedia
                className={classes.media}
                image={`${imagesUrl}${movie.image}`}
                component="img"
            />
            <CardContent>
                <Typography gutterBottom variant="h5" component="h2" className={classes.cardElement}>
                    {movie.title}
                </Typography>
                <Typography variant="body1" component="p" className={classes.cardElement}>
                    {movie.overview}
                </Typography>
                <Button variant="contained" color="secondary" className={classes.cardElement}>
                    Detalles
                </Button>
            </CardContent>
        </Card>
    );
}

export default MovieCard;