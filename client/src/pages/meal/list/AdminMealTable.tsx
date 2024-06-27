import React, { useEffect, useState } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Button, Box, IconButton, Modal, TextField, Select, MenuItem, InputLabel, FormControl, Typography, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from '@mui/material';
import axios from 'axios';
import { useAuth } from '../../../components/provider/AuthProvider';
import { environment } from '../../../environments/environment';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import CloseIcon from '@mui/icons-material/Close';
import { useNotification } from '../../../components/provider/NotificationContext';

const mealTypes = [
    { value: 0, label: 'Breakfast' },
    { value: 1, label: 'Lunch' },
    { value: 2, label: 'Dinner' },
];

const dietTypes = [
    { value: 0, label: 'NoDiet' },
    { value: 1, label: 'LactoVegetarian' },
    { value: 2, label: 'OvoVegetarian' },
    { value: 3, label: 'Paleo' },
    { value: 4, label: 'Primal' },
    { value: 5, label: 'Pescetarian' },
    { value: 6, label: 'Vegan' },
    { value: 7, label: 'Vegetarian' },
    { value: 8, label: 'Whole30' },
];

const allergyTypes = [
    { value: 0, label: 'Dairy' },
    { value: 1, label: 'Egg' },
    { value: 2, label: 'Gluten' },
    { value: 3, label: 'Peanut' },
    { value: 4, label: 'Seafood' },
    { value: 5, label: 'Sesame' },
    { value: 6, label: 'Shellfish' },
    { value: 7, label: 'Soy' },
    { value: 8, label: 'TreeNut' },
    { value: 9, label: 'Wheat' },
    { value: 10, label: 'Corn' },
];

interface Nutrients {
    calories: number;
    protein: number;
    carbohydrates: number;
    fat: number;
}

interface Meal {
    mealId: number;
    mealName: string;
    mealType: number;
    nutrients: Nutrients;
    preparationInstructions: string;
    estimatedPreparationTimeInMinutes: number;
    image: string;
    ingredients: string[];
    diets: number[];
    allergies: number[];
}

interface PagedResult<T> {
    items: T[];
    totalCount: number;
    pageSize: number;
    pageNumber: number;
}

const AdminMealTable: React.FC = () => {
    const { token } = useAuth();
    const { notify } = useNotification();
    const [meals, setMeals] = useState<Meal[]>([]);
    const [editMeal, setEditMeal] = useState<Meal | null>(null);
    const [newImage, setNewImage] = useState<File | null>(null);
    const [deleteMealId, setDeleteMealId] = useState<number | null>(null);

    useEffect(() => {
        const fetchMeals = async () => {
            try {
                const response = await axios.get<PagedResult<Meal>>(`${environment.apiUrl}Meal`, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                setMeals(response.data.items);
            } catch (error) {
                console.error('Failed to fetch meals:', error);
                notify('Failed to fetch meals', 'error');
            }
        };

        fetchMeals();
    }, [token]);

    const handleDelete = async () => {
        if (deleteMealId !== null) {
            try {
                await axios.delete(`${environment.apiUrl}Meal/${deleteMealId}`, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                setMeals(meals.filter(meal => meal.mealId !== deleteMealId));
                notify('Meal deleted successfully', 'success');
                setDeleteMealId(null); 
            } catch (error) {
                console.error('Failed to delete meal:', error);
                notify('Failed to delete meal', 'error');
            }
        }
    };

    const handleEdit = (meal: Meal) => {
        setEditMeal(meal);
        setNewImage(null);
    };

    const handleSave = async () => {
        if (editMeal) {
            const formData = new FormData();
            formData.append('mealId', editMeal.mealId.toString());
            formData.append('mealName', editMeal.mealName);
            formData.append('mealType', editMeal.mealType.toString());
            formData.append('nutrients.calories', editMeal.nutrients.calories.toString());
            formData.append('nutrients.protein', editMeal.nutrients.protein.toString());
            formData.append('nutrients.carbohydrates', editMeal.nutrients.carbohydrates.toString());
            formData.append('nutrients.fat', editMeal.nutrients.fat.toString());
            formData.append('preparationInstructions', editMeal.preparationInstructions);
            formData.append('estimatedPreparationTimeInMinutes', editMeal.estimatedPreparationTimeInMinutes.toString());
            editMeal.ingredients.forEach((ingredient, index) => {
                formData.append(`ingredients[${index}]`, ingredient);
            });
            editMeal.diets.forEach((diet, index) => {
                formData.append(`diets[${index}]`, diet.toString());
            });
            editMeal.allergies.forEach((allergy, index) => {
                formData.append(`allergies[${index}]`, allergy.toString());
            });
            if (newImage) {
                formData.append('image', newImage);
            }

            try {
                await axios.put(`${environment.apiUrl}Meal/${editMeal.mealId}`, formData, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                setMeals(meals.map(meal => (meal.mealId === editMeal.mealId ? editMeal : meal)));
                setEditMeal(null);
                notify('Meal updated successfully', 'success');
            } catch (error) {
                console.error('Failed to save meal:', error);
                notify('Failed to save meal', 'error');
            }
        }
    };

    const handleCloseModal = () => {
        setEditMeal(null);
    };

    const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            setNewImage(event.target.files[0]);
        }
    };

    const handleOpenDeleteDialog = (mealId: number) => {
        setDeleteMealId(mealId);
    };

    const handleCloseDeleteDialog = () => {
        setDeleteMealId(null);
    };

    return (
        <Box sx={{ padding: 2 }}>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Name</TableCell>
                            <TableCell>Type</TableCell>
                            <TableCell>Calories</TableCell>
                            <TableCell>Protein</TableCell>
                            <TableCell>Carbs</TableCell>
                            <TableCell>Fat</TableCell>
                            <TableCell>Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {meals.map((meal) => (
                            <TableRow key={meal.mealId}>
                                <TableCell>{meal.mealName}</TableCell>
                                <TableCell>{mealTypes[meal.mealType].label}</TableCell>
                                <TableCell>{meal.nutrients.calories}</TableCell>
                                <TableCell>{meal.nutrients.protein}</TableCell>
                                <TableCell>{meal.nutrients.carbohydrates}</TableCell>
                                <TableCell>{meal.nutrients.fat}</TableCell>
                                <TableCell>
                                    <IconButton onClick={() => handleEdit(meal)} color="primary">
                                        <EditIcon />
                                    </IconButton>
                                    <IconButton onClick={() => handleOpenDeleteDialog(meal.mealId)} color="secondary">
                                        <DeleteIcon />
                                    </IconButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <Modal
                open={!!editMeal}
                onClose={handleCloseModal}
                aria-labelledby="edit-meal-modal-title"
                aria-describedby="edit-meal-modal-description"
            >
                <Box sx={{
                    position: 'absolute' as 'absolute',
                    top: '50%',
                    left: '50%',
                    transform: 'translate(-50%, -50%)',
                    width: '80%',
                    maxHeight: '90%',
                    bgcolor: 'background.paper',
                    border: '2px solid #000',
                    boxShadow: 24,
                    p: 4,
                    overflowY: 'auto',
                }}>
                    <IconButton
                        aria-label="close"
                        onClick={handleCloseModal}
                        sx={{
                            position: 'absolute',
                            right: 8,
                            top: 8,
                            color: (theme) => theme.palette.grey[500],
                        }}
                    >
                        <CloseIcon />
                    </IconButton>
                    {editMeal && (
                        <>
                            <Typography id="edit-meal-modal-title" variant="h4" component="h2" align="center" sx={{ backgroundColor: '#00695c', color: '#fff', padding: '8px 0', marginBottom: '20px' }}>
                                Edit Meal
                            </Typography>
                            <TextField
                                fullWidth
                                margin="normal"
                                label="Name"
                                value={editMeal.mealName}
                                onChange={(e) => setEditMeal({ ...editMeal, mealName: e.target.value })}
                            />
                            <FormControl fullWidth margin="normal">
                                <InputLabel>Type</InputLabel>
                                <Select
                                    value={editMeal.mealType}
                                    onChange={(e) => setEditMeal({ ...editMeal, mealType: e.target.value as number })}
                                >
                                    {mealTypes.map((option) => (
                                        <MenuItem key={option.value} value={option.value}>
                                            {option.label}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                            <TextField
                                fullWidth
                                margin="normal"
                                label="Calories"
                                type="number"
                                value={editMeal.nutrients.calories}
                                onChange={(e) => setEditMeal({ ...editMeal, nutrients: { ...editMeal.nutrients, calories: Number(e.target.value) } })}
                            />
                            <TextField
                                fullWidth
                                margin="normal"
                                label="Protein"
                                type="number"
                                value={editMeal.nutrients.protein}
                                onChange={(e) => setEditMeal({ ...editMeal, nutrients: { ...editMeal.nutrients, protein: Number(e.target.value) } })}
                            />
                            <TextField
                                fullWidth
                                margin="normal"
                                label="Carbohydrates"
                                type="number"
                                value={editMeal.nutrients.carbohydrates}
                                onChange={(e) => setEditMeal({ ...editMeal, nutrients: { ...editMeal.nutrients, carbohydrates: Number(e.target.value) } })}
                            />
                            <TextField
                                fullWidth
                                margin="normal"
                                label="Fat"
                                type="number"
                                value={editMeal.nutrients.fat}
                                onChange={(e) => setEditMeal({ ...editMeal, nutrients: { ...editMeal.nutrients, fat: Number(e.target.value) } })}
                            />
                            <TextField
                                fullWidth
                                margin="normal"
                                label="Preparation Instructions"
                                value={editMeal.preparationInstructions}
                                onChange={(e) => setEditMeal({ ...editMeal, preparationInstructions: e.target.value })}
                            />
                            <TextField
                                fullWidth
                                margin="normal"
                                label="Estimated Preparation Time (minutes)"
                                type="number"
                                value={editMeal.estimatedPreparationTimeInMinutes}
                                onChange={(e) => setEditMeal({ ...editMeal, estimatedPreparationTimeInMinutes: Number(e.target.value) })}
                            />
                            <Box>
                                <Typography variant="h6" gutterBottom>
                                    Ingredients
                                </Typography>
                                {editMeal.ingredients.map((ingredient, index) => (
                                    <TextField
                                        key={index}
                                        fullWidth
                                        margin="normal"
                                        label={`Ingredient ${index + 1}`}
                                        value={ingredient}
                                        onChange={(e) => {
                                            const newIngredients = [...editMeal.ingredients];
                                            newIngredients[index] = e.target.value;
                                            setEditMeal({ ...editMeal, ingredients: newIngredients });
                                        }}
                                    />
                                ))}
                                <Button
                                    variant="contained"
                                    onClick={() => setEditMeal({ ...editMeal, ingredients: [...editMeal.ingredients, ''] })}
                                    sx={{ mt: 2 }}
                                >
                                    Add Ingredient
                                </Button>
                            </Box>
                            <Box>
                                <Typography variant="h6" gutterBottom>
                                    Diets
                                </Typography>
                                <FormControl fullWidth margin="normal">
                                    <InputLabel>Diets</InputLabel>
                                    <Select
                                        multiple
                                        value={editMeal.diets}
                                        onChange={(e) => setEditMeal({ ...editMeal, diets: e.target.value as number[] })}
                                    >
                                        {dietTypes.map((option) => (
                                            <MenuItem key={option.value} value={option.value}>
                                                {option.label}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            </Box>
                            <Box>
                                <Typography variant="h6" gutterBottom>
                                    Allergies
                                </Typography>
                                <FormControl fullWidth margin="normal">
                                    <InputLabel>Allergies</InputLabel>
                                    <Select
                                        multiple
                                        value={editMeal.allergies}
                                        onChange={(e) => setEditMeal({ ...editMeal, allergies: e.target.value as number[] })}
                                    >
                                        {allergyTypes.map((option) => (
                                            <MenuItem key={option.value} value={option.value}>
                                                {option.label}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            </Box>
                            <Box>
                                <Typography variant="h6" gutterBottom>
                                    Image
                                </Typography>
                                <Button variant="contained" component="label">
                                    Upload New Image
                                    <input
                                        type="file"
                                        hidden
                                        onChange={handleImageChange}
                                    />
                                </Button>
                            </Box>
                            <Box sx={{ display: 'flex', justifyContent: 'space-between', marginTop: '20px' }}>
                                <Button variant="contained" color="primary" onClick={handleSave} startIcon={<SaveIcon />}>
                                    Save
                                </Button>
                                <Button variant="contained" color="secondary" onClick={handleCloseModal} startIcon={<CloseIcon />}>
                                    Cancel
                                </Button>
                            </Box>
                        </>
                    )}
                </Box>
            </Modal>
            <Dialog
                open={deleteMealId !== null}
                onClose={handleCloseDeleteDialog}
                aria-labelledby="delete-meal-dialog-title"
                aria-describedby="delete-meal-dialog-description"
            >
                <DialogTitle id="delete-meal-dialog-title">Confirm Delete</DialogTitle>
                <DialogContent>
                    <DialogContentText id="delete-meal-dialog-description">
                        Are you sure you want to delete this meal?
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCloseDeleteDialog} color="primary">
                        Cancel
                    </Button>
                    <Button onClick={handleDelete} color="secondary" autoFocus>
                        Delete
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
};

export default AdminMealTable;
