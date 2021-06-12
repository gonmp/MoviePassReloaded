import * as React from 'react';
import IShow from '../../../models/show';
import Navbar from '../../shared/Navbar';
import ShowService from '../../../services/ShowService';
import MovieListings from '../../shared/MovieListings';

const Landing = () => {
    const [shows, setShows] = React.useState<IShow[]>([]);

    const getShows = async () => {
        const response = await ShowService.getNowPlaying();
        if (response.status === 200) {
            setShows(response.data);
        }
    }

    React.useEffect(() => {
        getShows();
    }, []);

    return (
        <>
            <Navbar />
            <MovieListings
                showsNowPlaying={shows}
            />
        </>
    );
}

export default Landing;