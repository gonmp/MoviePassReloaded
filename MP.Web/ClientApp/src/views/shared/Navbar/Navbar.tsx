import * as React from 'react';
import { AppBar, makeStyles, Toolbar, Typography } from '@material-ui/core';

const useStyles = makeStyles({
    rightToolBar: {
        marginLeft: "auto",
        marginRight: -12
    },
    navMenuItem: {
        display: "inline-block",
        marginRight: 25,
        marginLeft: 25
    }
});

const Navbar = () => {
    const classes = useStyles();

    return (
        <AppBar position="static" color="secondary">
            <Toolbar>
                <section className={classes.rightToolBar}>
                    <Typography className={classes.navMenuItem}>
                        Cartelera
                    </Typography>
                    <Typography className={classes.navMenuItem}>
                        Horarios
                    </Typography>
                    <Typography className={classes.navMenuItem}>
                        Promociones
                    </Typography>
                    <Typography className={classes.navMenuItem}>
                        Proximos estrenos
                    </Typography>
                    <Typography className={classes.navMenuItem}>
                        Contacto
                    </Typography>
                </section>
            </Toolbar>
        </AppBar>
    );
}

export default Navbar;