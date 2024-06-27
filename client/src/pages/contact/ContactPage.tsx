import React, { useState } from 'react';
import { Box, Typography, TextField, Button, Grid, Paper, IconButton } from '@mui/material';
import ChatIcon from '@mui/icons-material/Chat';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';

const ContactPage: React.FC = () => {
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [subject, setSubject] = useState('');
    const [message, setMessage] = useState('');

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
     
        alert('Form submitted successfully!');
        setName('');
        setEmail('');
        setSubject('');
        setMessage('');
    };

    return (
        <Box sx={{ padding: 4, backgroundColor: '#f9fafc' }}>
            <Typography variant="h4" align="center" gutterBottom sx={{ color: '#333', fontWeight: 'bold' }}>
                We are here for you, contact us at <span style={{ color: '#f57c00' }}>anytime</span>
            </Typography>
            <Typography align="center" sx={{ marginBottom: 4, color: '#666' }}>
                Have any questions about our services or just want to talk with us? Please reach out.
            </Typography>
            <Grid container spacing={4} justifyContent="center">
                {/* <Grid item xs={12} md={4}>
                    <Paper sx={{ padding: 3, textAlign: 'center', borderRadius: '12px' }}>
                        <IconButton color="primary" sx={{ fontSize: 40, backgroundColor: '#e0f7fa', marginBottom: 2 }}>
                            <ChatIcon sx={{ fontSize: 40, color: '#00796b' }} />
                        </IconButton>
                        <Typography variant="h6" sx={{ fontWeight: 'bold' }}>
                            Chat Now
                        </Typography>
                        <Typography sx={{ color: '#666', marginBottom: 2 }}>
                            Right from this website
                        </Typography>
                        <Button variant="contained" sx={{ backgroundColor: '#ff9800', '&:hover': { backgroundColor: '#fb8c00' } }}>
                            Start Chat
                        </Button>
                    </Paper>
                </Grid> */}
                <Grid item xs={12} md={4}>
                    <Paper sx={{ padding: 3, textAlign: 'center', borderRadius: '12px' }}>
                        <IconButton color="primary" sx={{ fontSize: 40, backgroundColor: '#f3e5f5', marginBottom: 2 }}>
                            <EmailIcon sx={{ fontSize: 40, color: '#8e24aa' }} />
                        </IconButton>
                        <Typography variant="h6" sx={{ fontWeight: 'bold', marginBottom: 1 }}>
                            Email Us
                        </Typography>
                        <Typography sx={{ color: '#666', marginBottom: 2 }}>
                            From your email app
                        </Typography>
                        <Typography variant="body1" sx={{ color: '#333', fontWeight: 'bold' }}>
                            lifestyle@gmail.com
                        </Typography>
                    </Paper>
                </Grid>
                <Grid item xs={12} md={4}>
                    <Paper sx={{ padding: 3, textAlign: 'center', borderRadius: '12px' }}>
                        <IconButton color="primary" sx={{ fontSize: 40, backgroundColor: '#ffebee', marginBottom: 2 }}>
                            <PhoneIcon sx={{ fontSize: 40, color: '#d32f2f' }} />
                        </IconButton>
                        <Typography variant="h6" sx={{ fontWeight: 'bold', marginBottom: 1 }}>
                            Call or Text us
                        </Typography>
                        <Typography sx={{ color: '#666', marginBottom: 2 }}>
                            From your phone
                        </Typography>
                        <Typography variant="body1" sx={{ color: '#333', fontWeight: 'bold' }}>
                            +40 (745) 492-1815
                        </Typography>
                    </Paper>
                </Grid>
            </Grid>
            {/* <Box sx={{ marginTop: 4, maxWidth: 600, margin: 'auto' }}>
                <Typography variant="h6" align="center" gutterBottom sx={{ fontWeight: 'bold' }}>
                    Send Us a Message
                </Typography>
                <Box component="form" onSubmit={handleSubmit} sx={{ backgroundColor: '#fff', padding: 3, borderRadius: '12px', boxShadow: 3 }}>
                    <TextField
                        label="Name"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        fullWidth
                        required
                        sx={{ marginBottom: 2 }}
                    />
                    <TextField
                        label="Email"
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        fullWidth
                        required
                        sx={{ marginBottom: 2 }}
                    />
                    <TextField
                        label="Subject"
                        value={subject}
                        onChange={(e) => setSubject(e.target.value)}
                        fullWidth
                        required
                        sx={{ marginBottom: 2 }}
                    />
                    <TextField
                        label="Message"
                        value={message}
                        onChange={(e) => setMessage(e.target.value)}
                        fullWidth
                        required
                        multiline
                        rows={4}
                        sx={{ marginBottom: 2 }}
                    />
                    <Button
                        type="submit"
                        variant="contained"
                        color="primary"
                        sx={{
                            backgroundColor: '#00695c',
                            '&:hover': {
                                backgroundColor: '#004d40',
                            },
                            color: '#fff',
                            padding: '10px 20px',
                            fontSize: '16px',
                            borderRadius: '5px',
                            boxShadow: '0 3px 5px rgba(0,0,0,0.2)',
                        }}
                        fullWidth
                    >
                        Send
                    </Button>
                </Box>
            </Box>
            <Typography align="center" sx={{ marginTop: 4, color: '#666' }}>
                We'll get back to you as soon as possible. Our team is available 8am-6pm on weekdays.
            </Typography> */}
        </Box>
    );
};

export default ContactPage;


