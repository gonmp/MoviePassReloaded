import * as React from 'react';
import IShow from '../../../models/show';
import MovieCard from '../MovieCard';

export interface IMovieListingsProps {
    showsNowPlaying: IShow[]
}

const MovieListings = ({ showsNowPlaying }: IMovieListingsProps) => {

    React.useEffect(() => { }, [showsNowPlaying]);
    return (
        <>
            {
                showsNowPlaying.map(show => (
                    <MovieCard
                        key={show.id}
                        movie={show.movie}
                    />))
            }        
        </>
    );
}

export default MovieListings;