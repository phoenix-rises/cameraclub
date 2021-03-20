import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';
import { ClubApi } from '../ClubApi';
import { AddPhotographerEntry } from './AddPhotographerEntry';
import { PhotographerEntry } from './PhotographerEntry';

export class CompetitionPhotographers extends Component {
    clubApi;

    constructor(props) {
        super(props);

        this.clubApi = new ClubApi();

        var competitionId = props.match.params.competitionId;

        this.state = {
            competitionId: competitionId,
            competitionInfo: { "name": "", "hasDigital": false, "hasPrint": false },
            photographers: [],
            photos: [],
            categories: [],
            newPhotoId: "0",
            error: false,
            loading: true,
            errorMessage: ""
        }

        this.loadEntriesState = this.loadEntriesState.bind(this);
        this.loadData = this.loadData.bind(this);
        this.save = this.save.bind(this);
        this.loadCategoryState = this.loadCategoryState.bind(this);
        this.showError = this.showError.bind(this);
        this.addPhoto = this.addPhoto.bind(this);
        this.removePhoto = this.removePhoto.bind(this);
        this.uploadPhoto = this.uploadPhoto.bind(this);
        this.addPhotographer = this.addPhotographer.bind(this);
        this.removePhotographer = this.removePhotographer.bind(this);
        this.handleTitleChange = this.handleTitleChange.bind(this);
        this.handleCategoryChange = this.handleCategoryChange.bind(this);
    }

    componentDidMount() {
        this.loadData();
    }

    loadData() {
        this.clubApi.load("GetCategories", this.showError, this.loadCategoryState);
    }

    loadCategoryState(categories) {
        this.setState({ "categories": categories });

        this.clubApi.load("GetCompetitionEntries?competitionId=" + this.state.competitionId, this.showError, this.loadEntriesState);
    }

    loadEntriesState(entryData) {
        entryData.photographers.forEach((p) => p.isDeleted = false);
        entryData.photos.forEach((p) => { p.isDeleted = false; p.isDigital = this.state.categories.find(c => c.id === p.categoryId).isDigital; });

        this.setState({
            loading: false,
            error: false,
            errorMessage: null,
            competitionInfo: entryData.competition,
            photographers: entryData.photographers,
            photos: entryData.photos
        });
    }

    async save() {
        var saveData = {
            "competitionId": this.state.competitionId,
            "photographers": this.state.photographers,
            "photos": this.state.photos
        };

        var saveResponse = await this.clubApi.saveWithPut("SaveCompetitionEntries", saveData);

        if (!saveResponse.ok) {
            this.showError("An error was encountered saving the data. Please try again.");
        }
    }

    showError(error) {
        console.log(error);
        this.setState({
            loading: false,
            error: true,
            errorMessage: error
        });
    }

    addPhotographer(newPhotographer) {
        let photographers = [...this.state.photographers];

        photographers.push(newPhotographer);

        this.setState({ "photographers": photographers });
    }

    removePhotographer(photographerId) {
        let photographers = [...this.state.photographers];

        var photographerToUpdate = photographers.find(p => p.id === photographerId);
        photographerToUpdate.isDeleted = true;

        this.setState({ "photographers": photographers });
    }

    addPhoto(photographerId) {
        var newId = this.state.newPhotoId - 1;
        var newPhoto = { "id": newId, "competitionId": this.state.competitionId, "photographerId": photographerId, "title": "", "categoryId": "1", "fileName": "", "storageId": "", "isDeleted": false };
        let photos = [...this.state.photos, newPhoto];

        this.setState({ "photos": photos, newPhotoId: newId });
    }

    removePhoto(photoId) {
        this.updatePhotoState(photoId, (photoToUpdate) => { photoToUpdate.isDeleted = true; });
    }

    async uploadPhoto(photoId, fileName, fileInfo) {
        var fileResponse = await this.clubApi.saveFormData("UploadPhotoFile", fileInfo);

        if (!fileResponse.ok) {
            this.showError("An error was encountered uploading the files. Please try again.");
            return;
        }

        var jsonResponse = await fileResponse.json();

        if (!jsonResponse.storageId) {
            this.showError("An error was encountered uploading the files. Please try again.");
            return;
        }

        this.updatePhotoState(photoId, (photoToUpdate) => { photoToUpdate.fileName = fileName; photoToUpdate.storageId = jsonResponse.storageId; });
    }

    handleTitleChange(newTitle, photoId) {
        this.updatePhotoState(photoId, (photoToUpdate) => { photoToUpdate.title = newTitle; });
    }

    handleCategoryChange(newCategory, photoId) {
        var isDigital = this.state.categories.find(c => c.id == newCategory).isDigital;

        this.updatePhotoState(photoId, (photoToUpdate) => { photoToUpdate.categoryId = newCategory; photoToUpdate.isDigital = isDigital; });
    }

    updatePhotoState(photoId, updateMethod) {
        let photos = [...this.state.photos];

        var photoToUpdate = photos.find(p => p.id === photoId);
        updateMethod(photoToUpdate);

        this.setState({ "photos": photos });
    }

    renderCompetitionEntries() {
        return (
            <>
                <Row>
                    <Col>
                        <h1 className="page-title">Entries for {this.state.competitionInfo.name} ({new Intl.DateTimeFormat('en-us').format(new Date(this.state.competitionInfo.date))})</h1>
                    </Col>
                </Row>
                <AddPhotographerEntry addPhotographer={this.addPhotographer} currentPhotographers={this.state.photographers} />
                <Row>
                    {this.state.photographers.filter(p => !p.isDeleted).map(photographer =>
                        <PhotographerEntry key={photographer.id} photographer={photographer} photos={this.state.photos} categories={this.state.categories}
                            handleTitleChange={this.handleTitleChange} handleCategoryChange={this.handleCategoryChange}
                            addPhoto={this.addPhoto} uploadPhoto={this.uploadPhoto} removePhoto={this.removePhoto} viewPhoto={this.viewPhoto} removePhotographer={this.removePhotographer} />
                    )}
                </Row>
                <Row className="top-margin-spacing">
                    <Col className="modal-footer">
                        <a className="btn btn-secondary" href={"/"}>Cancel</a>
                        <button className="btn btn-primary" onClick={(e) => { e.preventDefault(); this.save(); }}>Save Changes</button>
                    </Col>
                </Row>
            </>
        );
    }

    render() {
        let contents = this.state.error
            ? <p>Error:  <span dangerouslySetInnerHTML={{ __html: this.state.errorMessage }}></span></p>
            : this.state.loading
                ? <p><em>Loading...</em></p>
                : this.renderCompetitionEntries();

        return (
            <div>
                {contents}
            </div>
        );
    }
}