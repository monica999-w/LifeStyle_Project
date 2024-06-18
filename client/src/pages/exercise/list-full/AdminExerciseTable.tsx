import React, { useEffect, useState } from 'react';
import {
    Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper,
    Button, Box, IconButton, TextField, Select, MenuItem, InputLabel, FormControl, Dialog, DialogTitle, DialogContent, Typography
} from '@mui/material';
import axios from 'axios';
import { useAuth } from '../../../components/provider/AuthProvider';
import { useNotification } from '../../../components/provider/NotificationContext';
import { environment } from '../../../environments/environment';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import CloseIcon from '@mui/icons-material/Close';

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
    { value: 6, label: 'MedicineBall' },
];

const majorMuscleTypes = [
    { value: 0, label: 'Arms' },
    { value: 1, label: 'Back' },
    { value: 2, label: 'Core' },
    { value: 3, label: 'Legs' },
    { value: 4, label: 'Shoulders' },
    { value: 5, label: 'FullBody' },
];

interface Exercise {
    id: number;
    name: string;
    durationInMinutes: number;
    description: string;
    videoLink: string;
    type: number;
    equipment: number;
    majorMuscle: number;
}

const AdminExerciseTable: React.FC = () => {
    const { token } = useAuth();
    const { notify } = useNotification();
    const [exercises, setExercises] = useState<Exercise[]>([]);
    const [editExercise, setEditExercise] = useState<Exercise | null>(null);
    const [openEditDialog, setOpenEditDialog] = useState(false);

    useEffect(() => {
        const fetchExercises = async () => {
            try {
                const response = await axios.get(`${environment.apiUrl}Exercise`, {
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${token}`
                    }
                });
                setExercises(response.data.items);
            } catch (error) {
                console.error('Failed to fetch exercises:', error);
                notify('Failed to fetch exercises', 'error');
            }
        };

        fetchExercises();
    }, [token]);

    const handleDelete = async (exerciseId: number) => {
        try {
            await axios.delete(`${environment.apiUrl}Exercise/${exerciseId}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setExercises(exercises.filter(exercise => exercise.id !== exerciseId));
            notify('Exercise deleted successfully', 'success');
        } catch (error) {
            console.error('Failed to delete exercise:', error);
            notify('Failed to delete exercise', 'error');
        }
    };

    const handleEdit = (exercise: Exercise) => {
        setEditExercise(exercise);
        setOpenEditDialog(true);
    };

    const handleSave = async () => {
        if (editExercise) {
            try {
                await axios.put(`${environment.apiUrl}Exercise/${editExercise.id}`, editExercise, {
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${token}`
                    }
                });
                setExercises(exercises.map(exercise => exercise.id === editExercise.id ? editExercise : exercise));
                setOpenEditDialog(false);
                setEditExercise(null);
                notify('Exercise updated successfully', 'success');
            } catch (error) {
                console.error('Failed to update exercise:', error);
                notify('Failed to update exercise', 'error');
            }
        }
    };

    const truncateText = (text: string, maxLength: number) => {
        if (text.length > maxLength) {
            return text.substring(0, maxLength) + '...';
        }
        return text;
    };

    return (
        <Box sx={{ padding: 2 }}>
            <Typography variant="h4" gutterBottom>Manage Exercises</Typography>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Name</TableCell>
                            <TableCell>Duration (min)</TableCell>
                            <TableCell>Description</TableCell>
                            <TableCell>Video Link</TableCell>
                            <TableCell>Type</TableCell>
                            <TableCell>Equipment</TableCell>
                            <TableCell>Major Muscle</TableCell>
                            <TableCell>Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {exercises.map((exercise) => (
                            <TableRow key={exercise.id}>
                                <TableCell>{exercise.name}</TableCell>
                                <TableCell>{exercise.durationInMinutes}</TableCell>
                                <TableCell>{truncateText(exercise.description, 30)}</TableCell>
                                <TableCell>{truncateText(exercise.videoLink, 30)}</TableCell>
                                <TableCell>{exerciseTypes.find(type => type.value === exercise.type)?.label}</TableCell>
                                <TableCell>{equipmentTypes.find(type => type.value === exercise.equipment)?.label}</TableCell>
                                <TableCell>{majorMuscleTypes.find(type => type.value === exercise.majorMuscle)?.label}</TableCell>
                                <TableCell>
                                    <IconButton color="primary" onClick={() => handleEdit(exercise)}>
                                        <EditIcon />
                                    </IconButton>
                                    <IconButton color="secondary" onClick={() => handleDelete(exercise.id)}>
                                        <DeleteIcon />
                                    </IconButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            {editExercise && (
                <Dialog open={openEditDialog} onClose={() => setOpenEditDialog(false)} maxWidth="md" fullWidth>
                    <DialogTitle>Edit Exercise</DialogTitle>
                    <DialogContent>
                        <Box display="flex" flexDirection="column" alignItems="center">
                            <TextField
                                label="Name"
                                value={editExercise.name}
                                onChange={(e) => setEditExercise({ ...editExercise, name: e.target.value })}
                                fullWidth
                                margin="normal"
                            />
                            <TextField
                                label="Duration (minutes)"
                                type="number"
                                value={editExercise.durationInMinutes}
                                onChange={(e) => setEditExercise({ ...editExercise, durationInMinutes: parseInt(e.target.value) })}
                                fullWidth
                                margin="normal"
                            />
                            <TextField
                                label="Description"
                                value={editExercise.description}
                                onChange={(e) => setEditExercise({ ...editExercise, description: e.target.value })}
                                fullWidth
                                margin="normal"
                            />
                            <TextField
                                label="Video Link"
                                value={editExercise.videoLink}
                                onChange={(e) => setEditExercise({ ...editExercise, videoLink: e.target.value })}
                                fullWidth
                                margin="normal"
                            />
                            <FormControl fullWidth margin="normal">
                                <InputLabel>Type</InputLabel>
                                <Select
                                    value={editExercise.type}
                                    onChange={(e) => setEditExercise({ ...editExercise, type: e.target.value as number })}
                                >
                                    {exerciseTypes.map((option) => (
                                        <MenuItem key={option.value} value={option.value}>
                                            {option.label}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                            <FormControl fullWidth margin="normal">
                                <InputLabel>Equipment</InputLabel>
                                <Select
                                    value={editExercise.equipment}
                                    onChange={(e) => setEditExercise({ ...editExercise, equipment: e.target.value as number })}
                                >
                                    {equipmentTypes.map((option) => (
                                        <MenuItem key={option.value} value={option.value}>
                                            {option.label}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                            <FormControl fullWidth margin="normal">
                                <InputLabel>Major Muscle</InputLabel>
                                <Select
                                    value={editExercise.majorMuscle}
                                    onChange={(e) => setEditExercise({ ...editExercise, majorMuscle: e.target.value as number })}
                                >
                                    {majorMuscleTypes.map((option) => (
                                        <MenuItem key={option.value} value={option.value}>
                                            {option.label}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                            <Box mt={2}>
                                <Button
                                    variant="contained"
                                    color="primary"
                                    onClick={handleSave}
                                    startIcon={<SaveIcon />}
                                    sx={{ marginRight: 1 }}
                                >
                                    Save
                                </Button>
                                <Button
                                    variant="contained"
                                    color="secondary"
                                    onClick={() => setOpenEditDialog(false)}
                                    startIcon={<CloseIcon />}
                                >
                                    Cancel
                                </Button>
                            </Box>
                        </Box>
                    </DialogContent>
                </Dialog>
            )}
        </Box>
    );
};

export default AdminExerciseTable;
