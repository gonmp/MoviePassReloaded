import axios from 'axios';
import IShow from '../models/show';

export default class ShowService {
    static getNowPlaying = async () => {
        try {
            return await axios.get('https://localhost:44370/api/shows/now-playing');
        } catch (error) {
            return error.response ? error.response : error.message;
        }
    };
}