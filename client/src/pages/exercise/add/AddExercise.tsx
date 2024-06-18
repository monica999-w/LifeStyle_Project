import React, { useState } from 'react';
import axios from 'axios';
import { TextField, Button, Box, MenuItem, Typography } from '@mui/material';
import { useAuth } from '../../../components/provider/AuthProvider';
import { useNotification } from '../../../components/provider/NotificationContext';
import { environment } from '../../../environments/environment';

const exerciseTypes = [
    { value: 0, label: 'Yoga' },
    { value: 1, label: 'Cardio' },
    { value: 2, label: 'Pilates' },
    { value: 3, label: 'CrossFit' },
    { value: 4, label: 'Aerobics' },
];

const equipmentTypes = [
    { value: 0, label: 'BodyWeight' },
    { value: 1, label: 'Band' },
    { value: 2, label: 'Barbell' },
    { value: 3, label: 'Dumbbell' },
    { value: 4, label: 'Kettlebell' },
    { value: 5, label: 'Machine' },
    { value: 6, label: 'Medicine Ball' }
];

const majorMuscleGroups = [
    { value: 0, label: 'Arms' },
    { value: 1, label: 'Back' },
    { value: 2, label: 'Core' },
    { value: 3, label: 'Legs' },
    { value: 4, label: 'Shoulders' },
    { value: 5, label: 'Full Body' }
];

const AddExercise: React.FC = () => {
    const { token } = useAuth();
    const { notify } = useNotification();
    const [name, setName] = useState('');
    const [durationInMinutes, setDurationInMinutes] = useState(0);
    const [description, setDescription] = useState('');
    const [videoLink, setVideoLink] = useState('');
    const [type, setType] = useState(0);
    const [equipment, setEquipment] = useState(0);
    const [majorMuscle, setMajorMuscle] = useState(0);
    const [error, setError] = useState('');
    const apiUrl = `${environment.apiUrl}Exercise`;

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        console.log("API URL: ", apiUrl);
        const exercise = {
            name,
            durationInMinutes,
            description,
            videoLink,
            type,
            equipment,
            majorMuscle
        };

        console.log('Sending Exercise:', exercise);

        try {
            await axios.post(apiUrl, exercise, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                }
            });
            notify('Exercise added successfully', 'success');
            setName('');
            setDurationInMinutes(0);
            setDescription('');
            setVideoLink('');
            setType(0);
            setEquipment(0);
            setMajorMuscle(0);
        } catch (error) {
            console.error(error);
            setError('Failed to add exercise.');
            notify('Failed to add exercise', 'error');
        }
    };

    return (
        <Box>
            <Typography variant="h5" component="h2">Add Exercise</Typography>
            <form onSubmit={handleSubmit}>
                <TextField
                    fullWidth
                    margin="normal"
                    label="Name"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    required
                />
                <TextField
                    fullWidth
                    margin="normal"
                    label="Duration (in minutes)"
                    type="number"
                    value={durationInMinutes}
                    onChange={(e) => setDurationInMinutes(parseInt(e.target.value, 10))}
                    required
                />
                <TextField
                    fullWidth
                    margin="normal"
                    label="Description"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    required
                />
                <TextField
                    fullWidth
                    margin="normal"
                    label="Video Link"
                    value={videoLink}
                    onChange={(e) => setVideoLink(e.target.value)}
                    required
                />
                <TextField
                    fullWidth
                    margin="normal"
                    select
                    label="Type"
                    value={type}
                    onChange={(e) => setType(parseInt(e.target.value, 10))}
                    required
                >
                    {exerciseTypes.map((option) => (
                        <MenuItem key={option.value} value={option.value}>
                            {option.label}
                        </MenuItem>
                    ))}
                </TextField>
                <TextField
                    fullWidth
                    margin="normal"
                    select
                    label="Equipment"
                    value={equipment}
                    onChange={(e) => setEquipment(parseInt(e.target.value, 10))}
                    required
                >
                    {equipmentTypes.map((option) => (
                        <MenuItem key={option.value} value={option.value}>
                            {option.label}
                        </MenuItem>
                    ))}
                </TextField>
                <TextField
                    fullWidth
                    margin="normal"
                    select
                    label="Major Muscle Group"
                    value={majorMuscle}
                    onChange={(e) => setMajorMuscle(parseInt(e.target.value, 10))}
                    required
                >
                    {majorMuscleGroups.map((option) => (
                        <MenuItem key={option.value} value={option.value}>
                            {option.label}
                        </MenuItem>
                    ))}
                </TextField>
                {error && <Typography color="error">{error}</Typography>}
                <Button type="submit" variant="contained" color="primary">Add Exercise</Button>
            </form>
        </Box>
    );
};

export default AddExercise;
