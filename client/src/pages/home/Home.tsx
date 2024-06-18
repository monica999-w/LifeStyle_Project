import React from 'react';
import { Typography, Box } from '@mui/material';

const Home: React.FC = () => {
    return (
        <Box display="flex" flexDirection="column" alignItems="center" justifyContent="center" height="100vh">
            <Typography variant="h2">Welcome to LifeStyle</Typography>
            <Typography variant="h6">Your go-to app for managing your health and lifestyle!</Typography>
        </Box>
    );
};

export default Home;
