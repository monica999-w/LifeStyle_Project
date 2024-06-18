import React, { useState } from 'react';
import { Typography, Box, Card, CardContent, Button, Grid, Dialog, DialogTitle, DialogContent } from '@mui/material';
import {  FaDumbbell, FaRunning } from 'react-icons/fa';
import './ExerciseList.css';

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

interface ExerciseListProps {
    exercises: Exercise[];
}

const getTypeString = (type: number) => {
    const types = [
         'Yoga',
         'Cardio',
         'Pilates',
         'CrossFit',
         'Aerobics',
    ];
    return types[type] ||  'Unknown' ;
};

const getEquipmentString = (equipment: number) => {
    const equipments = [
        'BodyWeight',
        'Band',
        'Barbell', 
        'Dumbbell',
        'Kettlebell',
        'Machine',
        'MedicineBall',
    ];
    return equipments[equipment] || 'Unknown' ;
};

const getMajorMuscleString = (majorMuscle: number) => {
    const muscles = [
        'Arms',
        'Back',
        'Core',
        'Legs',
        'Shoulders',
        'FullBody'
    ];
    return muscles[majorMuscle] || 'Unknown';
};

const ExerciseList: React.FC<ExerciseListProps> = ({ exercises }) => {
    const [open, setOpen] = useState(false);
    const [selectedExercise, setSelectedExercise] = useState<Exercise | null>(null);

    const handleClickOpen = (exercise: Exercise) => {
        setSelectedExercise(exercise);
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
        setSelectedExercise(null);
    };

    return (
        <Box >
            <Typography variant="h4" gutterBottom>Exercise</Typography>
            <Grid container spacing={2}>
                {exercises.map(exercise => (
                    <Grid item key={exercise.id} xs={12} sm={6} md={4}>
                        <Card className="exercise-card" variant="outlined">
                            <CardContent>
                                <Typography variant="h5" component="div">
                                    {exercise.name}
                                </Typography>
                                <Typography color="text.secondary">
                                    {`Duration: ${exercise.durationInMinutes} mins`}
                                </Typography>
                                {exercise.videoLink && (
                                    <iframe
                                        width="100%"
                                        height="200"
                                        src={exercise.videoLink.replace("watch?v=", "embed/")}
                                        title={exercise.name}
                                        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                        allowFullScreen
                                    ></iframe>
                                )}
                                <Button variant="contained" color="primary" className="read-more-button" onClick={() => handleClickOpen(exercise)}>
                                    Read More
                                </Button>
                            </CardContent>
                        </Card>
                    </Grid>
                ))}
            </Grid>
            {selectedExercise && (
                <Dialog open={open} onClose={handleClose} maxWidth="md" fullWidth>
                    <DialogTitle className="dialog-title">{selectedExercise.name}</DialogTitle>
                    <DialogContent>
                        <Box display="flex" flexDirection="column" alignItems="center">
                            {selectedExercise.videoLink && (
                                <iframe
                                    width="80%"
                                    height="300"
                                    src={selectedExercise.videoLink.replace("watch?v=", "embed/")}
                                    title={selectedExercise.name}
                                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                    allowFullScreen
                                ></iframe>
                            )}
                            <Box className="detail-section">
                            <Typography component="div" align="right"><FaRunning  color='#2a9d8f'/>
                                {`Type: ${getTypeString(selectedExercise.type)}`} 
                             </Typography>
                            <Typography className="type" component="div"> < FaDumbbell color='#2a9d8f'/>
                                {`Equipment: ${getEquipmentString(selectedExercise.equipment)}`}
                               
                            </Typography>
                            <Typography className="type" component="div">
                                {`Major Muscle: ${getMajorMuscleString(selectedExercise.majorMuscle)}`}
                            </Typography> 
                            </Box>
                            <Typography variant="body1" component="div" className="description-text">
                                {selectedExercise.description}
                            </Typography>
                           
                        </Box>
                    </DialogContent>
                </Dialog>
            )}
        </Box>
    );
};

export default ExerciseList;
