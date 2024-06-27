import React, { useState, useEffect } from 'react';
import { Typography, Box, Card, CardContent, Button, Grid, Pagination, Select, MenuItem, FormControl, InputLabel, SelectChangeEvent, Divider, TextField, IconButton } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../../../components/provider/AuthProvider';
import { useNotification } from '../../../components/provider/NotificationContext';
import { environment } from '../../../environments/environment';
import FilterListIcon from '@mui/icons-material/FilterList';
import SearchIcon from '@mui/icons-material/Search';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import FavoriteIcon from '@mui/icons-material/Favorite';
import '../list-sort/ExerciseList.css';

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

interface PagedResult<T> {
    items: T[];
    totalCount: number;
    pageSize: number;
    pageNumber: number;
}

const ExerciseListFull: React.FC = () => {
    const { token, email } = useAuth();
    const { notify } = useNotification();
    const navigate = useNavigate();
    const [, setExercises] = useState<Exercise[]>([]);
    const [filteredExercises, setFilteredExercises] = useState<Exercise[]>([]);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [filter, setFilter] = useState({ type: '', equipment: '', majorMuscle: '' });
    const [isFilterApplied, setIsFilterApplied] = useState(false);
    const [searchTerm, setSearchTerm] = useState('');
    const [plannerExercises, setPlannerExercises] = useState<number[]>([]);

    const fetchExercises = async () => {
        try {
            const response = await axios.get<PagedResult<Exercise>>(`${environment.apiUrl}Exercise`, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                },
                params: {
                    pageNumber: page,
                    pageSize: 6
                }
            });

            console.log('Fetched exercises:', response.data);

            const totalCount = response.data.totalCount || 0;
            const calculatedTotalPages = Math.ceil(totalCount / 6);

            setExercises(response.data.items);
            setFilteredExercises(response.data.items);
            setTotalPages(calculatedTotalPages);
        } catch (error) {
            setExercises([]);
            setFilteredExercises([]);
            setTotalPages(1);
        }
    };

    const fetchPlanner = async () => {
        try {
            const date = new Date().toISOString().split('T')[0];
            const response = await axios.get(`${environment.apiUrl}Planner/user/${email}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                },
                params: { date }
            });

            console.log('Fetched planner exercises:', response.data);

            const exerciseIds = response.data.exercises.map((exercise: Exercise) => exercise.id);
            setPlannerExercises(exerciseIds);
        } catch (error) {
            console.error('Failed to fetch planner:', error);
           
        }
    };

    useEffect(() => {
        fetchExercises();
    }, [page, token]);

    useEffect(() => {
        fetchPlanner(); 
    }, [token, email]);

    useEffect(() => {
        if (searchTerm === '') {
            fetchExercises();
        }
    }, [searchTerm]);

    const handleFilterChange = async (e: SelectChangeEvent) => {
        const newFilter = { ...filter, [e.target.name]: e.target.value };
        setFilter(newFilter);
        setIsFilterApplied(true);

        try {
            const response = await axios.get(`${environment.apiUrl}Exercise/filter`, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                },
                params: {
                    ...newFilter,
                    pageNumber: 1,
                    pageSize: 6
                }
            });

            console.log('Filtered exercises:', response.data);

            const totalCount = response.data.totalCount || 0;
            const calculatedTotalPages = Math.ceil(totalCount / 6);

            setFilteredExercises(response.data);
            setTotalPages(calculatedTotalPages);
            setPage(1);
        } catch (error) {
            console.error('Failed to filter exercises:', error);
            notify('Failed to filter exercises', 'error');
            setFilteredExercises([]);
            setTotalPages(1);
        }
    };

    const handleClearFilter = async () => {
        setFilter({ type: '', equipment: '', majorMuscle: '' });
        setIsFilterApplied(false);
        await fetchExercises();
    };

    const handlePageChange = async (_event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value);
        if (isFilterApplied) {
            const response = await axios.get<PagedResult<Exercise>>(`${environment.apiUrl}Exercise/filter`, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                },
                params: {
                    ...filter,
                    pageNumber: value,
                    pageSize: 10
                }
            });

            console.log('Paged filtered exercises:', response.data);

            const totalCount = response.data.totalCount || 0;
            const calculatedTotalPages = Math.ceil(totalCount / 6);

            setFilteredExercises(response.data.items);
            setTotalPages(calculatedTotalPages);
        } else {
            await fetchExercises();
        }
    };

    const handleSearch = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (searchTerm.trim() === '') {
            fetchExercises();
            return;
        }

        try {
            const response = await axios.get(`${environment.apiUrl}Exercise/search`, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                },
                params: { searchTerm, pageNumber: 1, pageSize: 6 }
            });

            console.log('Searched exercises:', response.data);

            const totalCount = response.data.totalCount || 0;
            const calculatedTotalPages = Math.ceil(totalCount / 6);

            setFilteredExercises(response.data);
            setTotalPages(calculatedTotalPages);
            setPage(1);
        } catch (error) {
            console.error('Failed to search exercises:', error);
            notify('Failed to search exercises', 'error');
            setFilteredExercises([]);
            setTotalPages(1);
        }
    };

    const handleAddToPlanner = async (exerciseId: number) => {
        try {
            const date = new Date().toISOString();
            const response = await axios.post(`${environment.apiUrl}Planner`, {
                profile: email,
                date,
                mealIds: [],
                exerciseIds: [exerciseId]
            }, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                }
            });

            console.log('Added to planner:', response.data);
            setPlannerExercises([...plannerExercises, exerciseId]);
            notify('Exercise added to planner', 'success');
        } catch (error) {
            console.error('Failed to add to planner:', error);
            notify('Failed to add to planner', 'error');
        }
    };

    const isExerciseInPlanner = (exerciseId: number) => plannerExercises.includes(exerciseId);

    return (
        <Box>
            <Box display="flex" justifyContent="center" alignItems="center" mt={2} mb={3}>
                <FilterListIcon sx={{ mr: 2, color: '#00695c' }} />
                <FormControl variant="outlined" sx={{ m: 1, minWidth: 120, borderRadius: '16px' }}>
                    <InputLabel>Type</InputLabel>
                    <Select name="type" value={filter.type} onChange={handleFilterChange} label="Type" sx={{ borderRadius: '16px' }}>
                        <MenuItem value=""><em>None</em></MenuItem>
                        <MenuItem value={0}>Yoga</MenuItem>
                        <MenuItem value={1}>Cardio</MenuItem>
                        <MenuItem value={2}>Pilates</MenuItem>
                        <MenuItem value={3}>CrossFit</MenuItem>
                        <MenuItem value={4}>Aerobics</MenuItem>
                    </Select>
                </FormControl>
                <FormControl variant="outlined" sx={{ m: 1, minWidth: 120, borderRadius: '16px' }}>
                    <InputLabel>Equipment</InputLabel>
                    <Select name="equipment" value={filter.equipment} onChange={handleFilterChange} label="Equipment" sx={{ borderRadius: '16px' }}>
                        <MenuItem value=""><em>None</em></MenuItem>
                        <MenuItem value={0}>BodyWeight</MenuItem>
                        <MenuItem value={1}>Band</MenuItem>
                        <MenuItem value={2}>Barbell</MenuItem>
                        <MenuItem value={3}>Dumbbell</MenuItem>
                        <MenuItem value={4}>Kettlebell</MenuItem>
                        <MenuItem value={5}>Machine</MenuItem>
                        <MenuItem value={6}>MedicineBall</MenuItem>
                    </Select>
                </FormControl>
                <FormControl variant="outlined" sx={{ m: 1, minWidth: 120, borderRadius: '16px' }}>
                    <InputLabel>Muscle</InputLabel>
                    <Select name="majorMuscle" value={filter.majorMuscle} onChange={handleFilterChange} label="Major Muscle" sx={{ borderRadius: '16px' }}>
                        <MenuItem value=""><em>None</em></MenuItem>
                        <MenuItem value={0}>Arms</MenuItem>
                        <MenuItem value={1}>Back</MenuItem>
                        <MenuItem value={2}>Core</MenuItem>
                        <MenuItem value={3}>Legs</MenuItem>
                        <MenuItem value={4}>Shoulders</MenuItem>
                        <MenuItem value={5}>FullBody</MenuItem>
                    </Select>
                </FormControl>
                {isFilterApplied && (
                    <Button variant="outlined" onClick={handleClearFilter} sx={{ m: 1 }}>
                        Clear
                    </Button>
                )}
            </Box>
            <Divider />
            <Box display="flex" justifyContent="center" alignItems="center" mt={2} mb={3}>
                <form onSubmit={handleSearch} style={{ display: 'flex', alignItems: 'center' }}>
                    <TextField
                        variant="outlined"
                        placeholder="Search exercise..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        sx={{ borderRadius: '16px', marginRight: '10px' }}
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
                            borderRadius: '16px',
                            boxShadow: '0 3px 5px rgba(0,0,0,0.2)',
                        }}
                    >
                        <SearchIcon />
                    </Button>
                </form>
            </Box>
            <Grid container spacing={2}>
                {filteredExercises && filteredExercises.length > 0 ? filteredExercises.map(exercise => (
                    <Grid item key={exercise.id} xs={12} sm={6} md={4}>
                        <Card className="exercise-card" variant="outlined">
                            <CardContent>
                                <Typography id="exercise-modal-title" variant="h6" component="h2" align="center" sx={{ backgroundColor: '#00695c', color: '#fff', padding: '8px 0', marginBottom: '20px' }}>
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
                                <Button
                                    variant="contained"
                                    color="primary"
                                    className="read-more-button"
                                    onClick={() => navigate(`/exercises/${exercise.id}`)}
                                    sx={{
                                        marginTop: '10px',
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
                                >
                                    Read More
                                </Button>
                                <IconButton
                                    onClick={() => handleAddToPlanner(exercise.id)}
                                    sx={{
                                        marginTop: '10px',
                                        color: isExerciseInPlanner(exercise.id) ? '#ff4081' : '#ddd',
                                        '&:hover': {
                                            color: isExerciseInPlanner(exercise.id) ? '#c60055' : '#ff4081',
                                        },
                                    }}
                                >
                                    {isExerciseInPlanner(exercise.id) ? <FavoriteIcon /> : <FavoriteBorderIcon />}
                                </IconButton>
                            </CardContent>
                        </Card>
                    </Grid>
                )) : (
                    <Typography variant="h6" align="center" sx={{ marginTop: 4, color: 'red' }}>
                        No exercises found.
                    </Typography>
                )}
            </Grid>
            <Box display="flex" justifyContent="center" mt={2}>
                <Pagination count={totalPages} page={page} onChange={handlePageChange} />
            </Box>
        </Box>
    );
};

export default ExerciseListFull;
