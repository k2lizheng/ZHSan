using GameFreeText;
using GameGlobal;
using GameManager;
using GameObjects;
using GameObjects.ArchitectureDetail;
using GameObjects.FactionDetail;
using GameObjects.SectionDetail;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tools;

namespace MarshalSectionDialogPlugin
{

    public class MarshalSectionDialog
    {
#pragma warning disable CS0169 // The field 'MarshalSectionDialog.AIDetail' is never used
        private SectionAIDetail AIDetail;
#pragma warning restore CS0169 // The field 'MarshalSectionDialog.AIDetail' is never used
        internal PlatformTexture AIDetailButtonDisplayTexture;
        internal Rectangle AIDetailButtonPosition;
        internal PlatformTexture AIDetailButtonSelectedTexture;
        internal PlatformTexture AIDetailButtonTexture;
        internal PlatformTexture ArchitectureListButtonDisplayTexture;
        internal Rectangle ArchitectureListButtonPosition;
        internal PlatformTexture ArchitectureListButtonSelectedTexture;
        internal PlatformTexture ArchitectureListButtonTexture;
        internal Point BackgroundSize;
        internal PlatformTexture BackgroundTexture;
        internal PlatformTexture CancelButtonDisabledTexture;
        internal PlatformTexture CancelButtonDisplayTexture;
        internal bool CancelButtonEnabled = true;
        internal Rectangle CancelButtonPosition;
        internal PlatformTexture CancelButtonSelectedTexture;
        internal PlatformTexture CancelButtonTexture;
        internal Point DisplayOffset;
        private Faction EditingFaction;
        private Section EditingSection;
        internal IGameFrame GameFramePlugin;
        private bool IsNew;
        private bool isShowing;
        internal List<LabelText> LabelTexts = new List<LabelText>();
        internal PlatformTexture OKButtonDisabledTexture;
        internal PlatformTexture OKButtonDisplayTexture;
        internal bool OKButtonEnabled;
        internal Rectangle OKButtonPosition;
        internal PlatformTexture OKButtonSelectedTexture;
        internal PlatformTexture OKButtonTexture;
        internal PlatformTexture OrientationButtonDisabledTexture;
        internal PlatformTexture OrientationButtonDisplayTexture;
        internal bool OrientationButtonEnabled = false;
        internal Rectangle OrientationButtonPosition;
        internal PlatformTexture OrientationButtonSelectedTexture;
        internal PlatformTexture OrientationButtonTexture;
#pragma warning disable CS0169 // The field 'MarshalSectionDialog.OrientationFaction' is never used
        private Faction OrientationFaction;
#pragma warning restore CS0169 // The field 'MarshalSectionDialog.OrientationFaction' is never used
#pragma warning disable CS0169 // The field 'MarshalSectionDialog.OrientationSection' is never used
        private Section OrientationSection;
#pragma warning restore CS0169 // The field 'MarshalSectionDialog.OrientationSection' is never used
#pragma warning disable CS0169 // The field 'MarshalSectionDialog.OrientationState' is never used
        private State OrientationState;
#pragma warning restore CS0169 // The field 'MarshalSectionDialog.OrientationState' is never used
        private Section OriginalSection;
        
        private GameObjectList SectionArchitectureList;
        internal ITabList TabListPlugin;

        internal void Draw()
        {
            Rectangle? sourceRectangle = null;
            CacheManager.Draw(this.BackgroundTexture, this.BackgroundDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
            foreach (LabelText text in this.LabelTexts)
            {
                text.Label.Draw(0.1999f);
                text.Text.Draw(0.1999f);
            }
            if (this.OKButtonEnabled)
            {
                sourceRectangle = null;
                CacheManager.Draw(this.OKButtonDisplayTexture, this.OKButtonDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            }
            else
            {
                sourceRectangle = null;
                CacheManager.Draw(this.OKButtonDisabledTexture, this.OKButtonDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            }
            if (this.CancelButtonEnabled)
            {
                sourceRectangle = null;
                CacheManager.Draw(this.CancelButtonDisplayTexture, this.CancelButtonDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            }
            else
            {
                sourceRectangle = null;
                CacheManager.Draw(this.CancelButtonDisabledTexture, this.CancelButtonDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            }
            sourceRectangle = null;
            CacheManager.Draw(this.ArchitectureListButtonDisplayTexture, this.ArchitectureListButtonDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            sourceRectangle = null;
            CacheManager.Draw(this.AIDetailButtonDisplayTexture, this.AIDetailButtonDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            if (this.OrientationButtonEnabled)
            {
                CacheManager.Draw(this.OrientationButtonDisplayTexture, this.OrientationButtonDisplayPosition, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            }
            else
            {
                CacheManager.Draw(this.OrientationButtonDisabledTexture, this.OrientationButtonDisplayPosition, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            }
        }

        internal void Initialize()
        {
            
        }

        private void OKOld()
        {
            if (this.IsNew)
            {
                foreach (Section section in this.EditingFaction.Sections)
                {
                    foreach (Architecture architecture in this.EditingSection.Architectures.GetList())
                    {
                        section.RemoveArchitecture(architecture);
                    }
                }
                foreach (Architecture architecture in this.EditingSection.Architectures)
                {
                    architecture.BelongedSection = this.EditingSection;
                }
                this.EditingFaction.AddSection(this.EditingSection);
                Session.Current.Scenario.Sections.AddSectionWithEvent(this.EditingSection);
            }
            else
            {
                if (this.OriginalSection.AIDetail.AutoRun != this.EditingSection.AIDetail.AutoRun)
                {
                    foreach (Architecture architecture in this.EditingSection.Architectures)
                    {
                        foreach (Routeway routeway in architecture.Routeways.GetList())
                        {
                            if (!(routeway.Building && (routeway.LastActivePointIndex >= 0)))
                            {
                                Session.Current.Scenario.RemoveRouteway(routeway);
                            }
                        }
                    }
                }
                this.OriginalSection.AIDetail = this.EditingSection.AIDetail;
                this.OriginalSection.OrientationFaction = this.EditingSection.OrientationFaction;
                this.OriginalSection.OrientationSection = this.EditingSection.OrientationSection;
                this.OriginalSection.OrientationState = this.EditingSection.OrientationState;
                this.OriginalSection.OrientationArchitecture = this.EditingSection.OrientationArchitecture;
                GameObjectList list = this.OriginalSection.Architectures.GetList();
                foreach (Architecture architecture in list)
                {
                    this.OriginalSection.RemoveArchitecture(architecture);
                }
                foreach (Section section in this.EditingFaction.Sections)
                {
                    foreach (Architecture architecture in this.EditingSection.Architectures.GetList())
                    {
                        section.RemoveArchitecture(architecture);
                    }
                }
                foreach (Architecture architecture in this.EditingSection.Architectures)
                {
                    this.OriginalSection.AddArchitecture(architecture);
                }
                Section anotherSection = this.EditingFaction.GetAnotherSection(this.OriginalSection);
                if (anotherSection != null)
                {
                    foreach (Architecture architecture in list)
                    {
                        if (!this.OriginalSection.HasArchitecture(architecture))
                        {
                            anotherSection.AddArchitecture(architecture);
                        }
                    }
                }
            }
            foreach (Section section in this.EditingFaction.Sections.GetList())
            {
                if (section.ArchitectureCount > 0)
                {
                    section.RefreshSectionName();
                }
                else
                {
                    foreach (Section section3 in this.EditingFaction.Sections.GetList())
                    {
                        if ((section3 != section) && (section3.OrientationSection == section))
                        {
                            foreach (SectionAIDetail detail in Session.Current.Scenario.GameCommonData.AllSectionAIDetails.SectionAIDetails.Values)
                            {
                                if (detail.OrientationKind == SectionOrientationKind.无)
                                {
                                    section3.AIDetail = detail;
                                    break;
                                }
                            }
                            section3.OrientationSection = null;
                        }
                    }
                    this.EditingFaction.RemoveSection(section);
                    Session.Current.Scenario.Sections.Remove(section);
                }
            }
            this.IsShowing = false;
        }

        private void RefreshLabelTextsDisplay()
        {
            foreach (LabelText text in this.LabelTexts)
            {
                text.Text.Text = StaticMethods.GetPropertyValue(this.EditingSection, text.PropertyName).ToString();
            }
        }

        private void RefreshOKButton()
        {
            this.OKButtonEnabled = 
                (
                    ((this.EditingSection.ArchitectureCount > 0) && 
                        (
                            (this.IsNew || (this.EditingFaction.SectionCount > 1)) || 
                            (this.EditingSection.ArchitectureCount == this.EditingFaction.ArchitectureCount)
                        )
                    ) && (
                        ((this.EditingFaction.SectionCount <= 1) || (this.EditingSection.ArchitectureCount < this.EditingFaction.ArchitectureCount)) &&
                        (this.EditingSection.AIDetail != null)
                    )
                ) && (
                    (
                        (this.EditingSection.AIDetail.OrientationKind == SectionOrientationKind.无) || 
                        ((this.EditingSection.AIDetail.OrientationKind == SectionOrientationKind.军区) && (this.EditingSection.OrientationSection != null)) ||
                        ((this.EditingSection.AIDetail.OrientationKind == SectionOrientationKind.势力) && (this.EditingSection.OrientationFaction != null)) ||
                        ((this.EditingSection.AIDetail.OrientationKind == SectionOrientationKind.州域) && (this.EditingSection.OrientationState != null)) ||
                        ((this.EditingSection.AIDetail.OrientationKind == SectionOrientationKind.建筑) && (this.EditingSection.OrientationArchitecture != null))
                    )
                );
        }

        private void RefreshOrientationButton()
        {
            this.OrientationButtonEnabled = (this.EditingSection.AIDetail != null) && (this.EditingSection.AIDetail.OrientationKind != SectionOrientationKind.无);
        }

        private void screen_OnMouseLeftUp(Point position)
        {
            if (Session.MainGame.mainGameScreen.PeekUndoneWork().Kind == UndoneWorkKind.Dialog)
            {
                if (StaticMethods.PointInRectangle(position, this.OKButtonDisplayPosition))
                {
                    if (this.OKButtonEnabled)
                    {
                        this.OK();
                    }
                }
                else if (StaticMethods.PointInRectangle(position, this.CancelButtonDisplayPosition))
                {
                    if (this.CancelButtonEnabled)
                    {
                        this.IsShowing = false;
                    }
                }
                else if (StaticMethods.PointInRectangle(position, this.ArchitectureListButtonDisplayPosition))
                {
                    this.ShowArchitectureListFrame();
                }
                else if (StaticMethods.PointInRectangle(position, this.AIDetailButtonDisplayPosition))
                {
                    this.ShowAIDetailFrame();
                }
                else if (StaticMethods.PointInRectangle(position, this.OrientationButtonDisplayPosition) && this.OrientationButtonEnabled)
                {
                    this.ShowOrientationFrame();
                }
            }
        }

        private void screen_OnMouseMove(Point position, bool leftDown)
        {
            if (Session.MainGame.mainGameScreen.PeekUndoneWork().Kind == UndoneWorkKind.Dialog)
            {
                if (StaticMethods.PointInRectangle(position, this.OKButtonDisplayPosition))
                {
                    if (this.OKButtonEnabled)
                    {
                        this.OKButtonDisplayTexture = this.OKButtonSelectedTexture;
                    }
                }
                else if (StaticMethods.PointInRectangle(position, this.CancelButtonDisplayPosition))
                {
                    if (this.CancelButtonEnabled)
                    {
                        this.CancelButtonDisplayTexture = this.CancelButtonSelectedTexture;
                    }
                }
                else if (StaticMethods.PointInRectangle(position, this.ArchitectureListButtonDisplayPosition))
                {
                    this.ArchitectureListButtonDisplayTexture = this.ArchitectureListButtonSelectedTexture;
                }
                else if (StaticMethods.PointInRectangle(position, this.AIDetailButtonDisplayPosition))
                {
                    this.AIDetailButtonDisplayTexture = this.AIDetailButtonSelectedTexture;
                }
                else if (StaticMethods.PointInRectangle(position, this.OrientationButtonDisplayPosition))
                {
                    if (this.OrientationButtonEnabled)
                    {
                        this.OrientationButtonDisplayTexture = this.OrientationButtonSelectedTexture;
                    }
                }
                else
                {
                    this.ArchitectureListButtonDisplayTexture = this.ArchitectureListButtonTexture;
                    this.AIDetailButtonDisplayTexture = this.AIDetailButtonTexture;
                    if (this.OKButtonEnabled)
                    {
                        this.OKButtonDisplayTexture = this.OKButtonTexture;
                    }
                    if (this.CancelButtonEnabled)
                    {
                        this.CancelButtonDisplayTexture = this.CancelButtonTexture;
                    }
                    if (this.OrientationButtonEnabled)
                    {
                        this.OrientationButtonDisplayTexture = this.OrientationButtonTexture;
                    }
                }
            }
        }

        private void screen_OnMouseRightDown(Point position)
        {
            if (Session.MainGame.mainGameScreen.PeekUndoneWork().Kind == UndoneWorkKind.Dialog)
            {
                this.IsShowing = false;
            }
        }

        internal void SetDisplayOffset(ShowPosition showPosition)
        {
            Rectangle rectDes = new Rectangle(0, 0, Session.MainGame.mainGameScreen.viewportSize.X, Session.MainGame.mainGameScreen.viewportSize.Y);
            Rectangle rect = new Rectangle(0, 0, this.BackgroundSize.X, this.BackgroundSize.Y);
            switch (showPosition)
            {
                case ShowPosition.Center:
                    rect = StaticMethods.GetCenterRectangle(rectDes, rect);
                    break;

                case ShowPosition.Top:
                    rect = StaticMethods.GetTopRectangle(rectDes, rect);
                    break;

                case ShowPosition.Left:
                    rect = StaticMethods.GetLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.Right:
                    rect = StaticMethods.GetRightRectangle(rectDes, rect);
                    break;

                case ShowPosition.Bottom:
                    rect = StaticMethods.GetBottomRectangle(rectDes, rect);
                    break;

                case ShowPosition.TopLeft:
                    rect = StaticMethods.GetTopLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.TopRight:
                    rect = StaticMethods.GetTopRightRectangle(rectDes, rect);
                    break;

                case ShowPosition.BottomLeft:
                    rect = StaticMethods.GetBottomLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.BottomRight:
                    rect = StaticMethods.GetBottomRightRectangle(rectDes, rect);
                    break;
            }
            this.DisplayOffset = new Point(rect.X, rect.Y);
            foreach (LabelText text in this.LabelTexts)
            {
                text.Label.DisplayOffset = this.DisplayOffset;
                text.Text.DisplayOffset = this.DisplayOffset;
            }
        }

        internal void SetFaction(Faction faction)
        {
            this.EditingFaction = faction;
        }

        internal void SetSectionOld(Section section)
        {
            this.OriginalSection = section;
            this.EditingSection = new Section();
            this.EditingSection.ID = Session.Current.Scenario.Sections.GetFreeGameObjectID();
            this.EditingSection.BelongedFaction = this.EditingFaction;
            if (section != null)
            {
                this.IsNew = false;
                foreach (Architecture architecture in section.Architectures)
                {
                    this.EditingSection.Architectures.Add(architecture);
                }
                this.EditingSection.Name = section.Name;
                this.EditingSection.AIDetail = section.AIDetail;
                this.EditingSection.OrientationFaction = section.OrientationFaction;
                this.EditingSection.OrientationSection = section.OrientationSection;
                this.EditingSection.OrientationState = section.OrientationState;
                this.EditingSection.OrientationArchitecture = section.OrientationArchitecture;                
                this.RefreshOKButton();
                this.RefreshOrientationButton();
                this.RefreshLabelTextsDisplay();
            }
            else
            {
                this.IsNew = true;
                this.RefreshLabelTextsDisplay();
            }
        }
        #region 军区处理-新建、重编、解散
        internal void SetSection(Section section)
        {
            this.OriginalSection = section;
            this.EditingSection = new Section();
            this.EditingSection.ID = Session.Current.Scenario.Sections.GetFreeGameObjectID();
            this.EditingSection.BelongedFaction = this.EditingFaction;

            if (section != null)
            {
                this.IsNew = false;

                // 复制原军区数据到编辑对象
                this.EditingSection.Name = section.Name;

                // 复制 AIDetail - 直接引用赋值（如果需要深拷贝，需要其他方式）
                this.EditingSection.AIDetail = section.AIDetail;

                this.EditingSection.OrientationFaction = section.OrientationFaction;
                this.EditingSection.OrientationSection = section.OrientationSection;
                this.EditingSection.OrientationState = section.OrientationState;
                this.EditingSection.OrientationArchitecture = section.OrientationArchitecture;

                // 复制建筑列表
                foreach (Architecture architecture in section.Architectures)
                {
                    this.EditingSection.Architectures.Add(architecture);
                }                

                this.RefreshOKButton();
                this.RefreshOrientationButton();
            }
            else
            {
                this.IsNew = true;
                // 设置默认的 AIDetail
                SetDefaultAIDetail();
            }

            this.RefreshLabelTextsDisplay();
        }

        private void SetDefaultAIDetail()
        {
            // 从游戏数据中获取默认的SectionAIDetail
            if (Session.Current.Scenario.GameCommonData?.AllSectionAIDetails?.SectionAIDetails != null)
            {
                foreach (SectionAIDetail detail in Session.Current.Scenario.GameCommonData.AllSectionAIDetails.SectionAIDetails.Values)
                {
                    if (detail.OrientationKind == SectionOrientationKind.无)
                    {
                        this.EditingSection.AIDetail = detail;
                        break;
                    }
                }
            }
        }

        private void OK()
        {
            try
            {
                if (this.IsNew)
                {
                    // 新建军区
                    CreateNewSection();
                }
                else
                {
                    // 修改现有军区
                    UpdateExistingSection();
                }

                // 清理空军区
                CleanEmptySections();

                this.IsShowing = false;
            }
            catch (Exception ex)
            {
                // 错误处理
                Console.WriteLine($"军区保存失败: {ex.Message}");
                // 可以回滚操作或显示错误消息
            }
        }

        private void CreateNewSection()
        {
            // 检查是否有重复军区名称
            if (this.EditingFaction.Sections.Any(s => s.Name == this.EditingSection.Name))
            {
                throw new InvalidOperationException("军区名称已存在");
            }

            // 从其他军区移除建筑
            foreach (Section section in this.EditingFaction.Sections)
            {
                foreach (Architecture architecture in this.EditingSection.Architectures.GetList())
                {
                    section.RemoveArchitecture(architecture);
                }
            }

            // 设置建筑的所属军区
            foreach (Architecture architecture in this.EditingSection.Architectures)
            {
                architecture.BelongedSection = this.EditingSection;
            }

            // 添加到势力和场景
            this.EditingFaction.AddSection(this.EditingSection);
            Session.Current.Scenario.Sections.AddSectionWithEvent(this.EditingSection);
        }

        private void UpdateExistingSection()
        {
            if (this.OriginalSection == null)
            {
                throw new InvalidOperationException("原军区不存在");
            }

            // 处理AutoRun变更
            if (this.OriginalSection.AIDetail.AutoRun != this.EditingSection.AIDetail.AutoRun)
            {
                HandleAutoRunChange();
            }

            // 更新基本属性
            this.OriginalSection.Name = this.EditingSection.Name;
            this.OriginalSection.AIDetail = this.EditingSection.AIDetail;
            this.OriginalSection.OrientationFaction = this.EditingSection.OrientationFaction;
            this.OriginalSection.OrientationSection = this.EditingSection.OrientationSection;
            this.OriginalSection.OrientationState = this.EditingSection.OrientationState;
            this.OriginalSection.OrientationArchitecture = this.EditingSection.OrientationArchitecture;

            // 更新建筑列表
            UpdateArchitectures();
        }

        private void HandleAutoRunChange()
        {
            foreach (Architecture architecture in this.EditingSection.Architectures)
            {
                foreach (Routeway routeway in architecture.Routeways.GetList())
                {
                    if (!(routeway.Building && routeway.LastActivePointIndex >= 0))
                    {
                        Session.Current.Scenario.RemoveRouteway(routeway);
                    }
                }
            }
        }

        private void UpdateArchitectures()
        {
            // 获取原建筑列表的副本
            var originalArchitectures = this.OriginalSection.Architectures.GetList();

            // 从原军区移除所有建筑
            foreach (Architecture architecture in originalArchitectures)
            {
                this.OriginalSection.RemoveArchitecture(architecture);
            }

            // 从其他军区移除新建筑列表中的建筑
            foreach (Section section in this.EditingFaction.Sections)
            {
                if (section != this.OriginalSection)
                {
                    foreach (Architecture architecture in this.EditingSection.Architectures.GetList())
                    {
                        section.RemoveArchitecture(architecture);
                    }
                }
            }

            // 将新建筑添加到原军区
            foreach (Architecture architecture in this.EditingSection.Architectures)
            {
                this.OriginalSection.AddArchitecture(architecture);
            }

            // 处理被移除的建筑：添加到其他军区
            var removedArchitectures = originalArchitectures
                .Where(a => !this.EditingSection.Architectures.HasGameObject(a))
                .GetList();

            if (removedArchitectures.Count > 0)
            {
                Section anotherSection = this.EditingFaction.GetAnotherSection(this.OriginalSection);
                if (anotherSection != null)
                {
                    foreach (Architecture architecture in removedArchitectures)
                    {
                        anotherSection.AddArchitecture(architecture);
                    }
                }
                else
                {
                    // 如果没有其他军区，这些建筑将成为无军区建筑
                    // 可能需要创建新的军区或者采取其他措施
                    Console.WriteLine($"有 {removedArchitectures.Count} 个建筑被移除但没有其他军区可以接收");
                }
            }
        }

        private void CleanEmptySections()
        {
            var sectionsToRemove = new List<Section>();

            // 收集空军区
            foreach (Section section in this.EditingFaction.Sections.GetList())
            {
                if (section.ArchitectureCount > 0)
                {
                    section.RefreshSectionName();
                }
                else
                {
                    sectionsToRemove.Add(section);
                }
            }

            // 处理依赖关系并移除空军区
            foreach (Section section in sectionsToRemove)
            {
                // 清除其他军区对该军区的指向
                foreach (Section otherSection in this.EditingFaction.Sections.GetList())
                {
                    if (otherSection.OrientationSection == section)
                    {
                        // 找到默认的SectionAIDetail（OrientationKind为无）
                        var defaultDetail = Session.Current.Scenario.GameCommonData.AllSectionAIDetails
                            .SectionAIDetails.Values
                            .FirstOrDefault(d => d.OrientationKind == SectionOrientationKind.无);

                        if (defaultDetail != null)
                        {
                            otherSection.AIDetail = defaultDetail;
                        }
                        otherSection.OrientationSection = null;
                    }
                }

                this.EditingFaction.RemoveSection(section);
                Session.Current.Scenario.Sections.Remove(section);
            }
        } 
        #endregion
        private void ShowAIDetailFrame()
        {
            this.TabListPlugin.InitialValues(Session.Current.Scenario.GameCommonData.AllSectionAIDetails.GetSectionAIDetailList(), this.EditingSection.AIDetail, InputManager.NowMouse.ScrollWheelValue, "");
            this.TabListPlugin.SetListKindByName("SectionAIDetail", true, false);
            this.TabListPlugin.SetSelectedTab("");
            this.GameFramePlugin.Kind = FrameKind.Section;
            this.GameFramePlugin.Function = FrameFunction.GetSection;
            this.GameFramePlugin.SetFrameContent(this.TabListPlugin.TabList, Session.MainGame.mainGameScreen.viewportSizeFull);
            this.GameFramePlugin.OKButtonEnabled = false;
            this.GameFramePlugin.CancelButtonEnabled = true;
            this.GameFramePlugin.SetOKFunction(delegate {
                this.EditingSection.AIDetail = this.TabListPlugin.SelectedItem as SectionAIDetail;
                switch (this.EditingSection.AIDetail.OrientationKind)
                {
                    case SectionOrientationKind.军区:
                    {
                        SectionList otherSections = this.EditingFaction.GetOtherSections(this.OriginalSection);
                        if (otherSections.Count == 1)
                        {
                            this.EditingSection.OrientationSection = otherSections[0] as Section;
                        }
                        break;
                    }
                    case SectionOrientationKind.势力:
                    {
                        GameObjectList diplomaticRelationListByFactionID = Session.Current.Scenario.DiplomaticRelations.GetDiplomaticRelationListByFactionID(this.EditingFaction.ID);
                        if (diplomaticRelationListByFactionID.Count == 1)
                        {
                            this.EditingSection.OrientationFaction = (diplomaticRelationListByFactionID[0] as DiplomaticRelation).GetDiplomaticFaction(this.EditingFaction.ID);
                        }
                        break;
                    }
                    case SectionOrientationKind.州域:
                    {
                        StateList states = Session.Current.Scenario.States;
                        if (states.Count == 1)
                        {
                            this.EditingSection.OrientationState = states[0] as State;
                        }
                        break;
                    }
                    case SectionOrientationKind.建筑:
                    {
                        ArchitectureList allArch = Session.Current.Scenario.Architectures;
                        ArchitectureList targetArch = new ArchitectureList();
                        foreach (Architecture a in allArch)
                        {
                            if (a.BelongedFaction != this.EditingFaction)
                            {
                                targetArch.Add(a);
                            }
                        }
                        if (targetArch.Count == 1)
                        {
                            this.EditingSection.OrientationArchitecture = targetArch[0] as Architecture;
                        }
                        break;
                    }
                }
                this.RefreshOKButton();
                if (this.IsNew)
                {
                    this.EditingSection.RefreshSectionName();
                }
                this.RefreshOrientationButton();
                this.RefreshLabelTextsDisplay();
            });
            this.GameFramePlugin.IsShowing = true;
        }

        private void ShowArchitectureListFrame()
        {
            this.TabListPlugin.InitialValues(this.EditingFaction.Architectures, this.EditingSection.Architectures, InputManager.NowMouse.ScrollWheelValue, "");
            this.TabListPlugin.SetListKindByName("Architecture", true, true);
            this.TabListPlugin.SetSelectedTab("");
            this.GameFramePlugin.Kind = FrameKind.Architecture;
            this.GameFramePlugin.Function = FrameFunction.GetFacilityToDemolish;
            this.GameFramePlugin.SetFrameContent(this.TabListPlugin.TabList, Session.MainGame.mainGameScreen.viewportSizeFull);
            this.GameFramePlugin.OKButtonEnabled = false;
            this.GameFramePlugin.CancelButtonEnabled = true;
            this.GameFramePlugin.SetOKFunction(delegate {
                this.SectionArchitectureList = this.TabListPlugin.SelectedItemList as GameObjectList;
                this.EditingSection.Architectures.Clear();
                foreach (Architecture architecture in this.SectionArchitectureList)
                {
                    this.EditingSection.Architectures.Add(architecture);
                }
                this.EditingSection.RefreshSectionName();
                this.RefreshOKButton();
                this.RefreshLabelTextsDisplay();
            });
            this.GameFramePlugin.IsShowing = true;
        }

        private void ShowOrientationFrame()
        {            
            if (this.EditingFaction == null || this.EditingSection.AIDetail == null || this.TabListPlugin == null || this.GameFramePlugin == null) return;

            try
            {
                GameDelegates.VoidFunction function = null;
                GameDelegates.VoidFunction function2 = null;
                GameDelegates.VoidFunction function3 = null;
                switch (this.EditingSection.AIDetail.OrientationKind)
                {
                    case SectionOrientationKind.军区:
                        this.TabListPlugin.InitialValues(this.EditingFaction.GetOtherSections(this.OriginalSection), null, InputManager.NowMouse.ScrollWheelValue, "");
                        this.TabListPlugin.SetListKindByName("Section", true, false);
                        this.TabListPlugin.SetSelectedTab("");
                        this.GameFramePlugin.Kind = FrameKind.TerrainDetail;
                        this.GameFramePlugin.Function = FrameFunction.Transport;
                        this.GameFramePlugin.SetFrameContent(this.TabListPlugin.TabList, Session.MainGame.mainGameScreen.viewportSizeFull);
                        this.GameFramePlugin.OKButtonEnabled = false;
                        this.GameFramePlugin.CancelButtonEnabled = true;
                        if (function == null)
                        {
                            function = delegate
                            {
                                this.EditingSection.OrientationFaction = null;
                                this.EditingSection.OrientationState = null;
                                this.EditingSection.OrientationArchitecture = null;
                                this.EditingSection.OrientationSection = this.TabListPlugin.SelectedItem as Section;
                                this.RefreshOKButton();
                                this.RefreshLabelTextsDisplay();
                            };
                        }
                        this.GameFramePlugin.SetOKFunction(function);
                        break;

                    case SectionOrientationKind.势力:
                        var list0 = Session.Current.Scenario.DiplomaticRelations.GetDiplomaticRelationDisplayListByFactionID(this.EditingFaction.ID);

                        if (list0 == null)
                        {
                            WebTools.TakeWarnMsg("list0 == null", "", new Exception("list0为空"));
                            return;
                        }

                        var list = list0.GetList();

                        this.TabListPlugin.InitialValues(list, null, InputManager.NowMouse.ScrollWheelValue, "");
                        this.TabListPlugin.SetListKindByName("DiplomaticRelation", true, false);
                        this.TabListPlugin.SetSelectedTab("");
                        this.GameFramePlugin.Kind = FrameKind.CastTargetKind;
                        this.GameFramePlugin.Function = FrameFunction.GetSectionToDemolish;
                        this.GameFramePlugin.SetFrameContent(this.TabListPlugin.TabList, Session.MainGame.mainGameScreen.viewportSizeFull);
                        this.GameFramePlugin.OKButtonEnabled = false;
                        this.GameFramePlugin.CancelButtonEnabled = true;
                        if (function2 == null)
                        {
                            function2 = delegate
                            {
                                this.EditingSection.OrientationSection = null;
                                this.EditingSection.OrientationState = null;
                                this.EditingSection.OrientationArchitecture = null;
                                DiplomaticRelationDisplay selectedItem = this.TabListPlugin.SelectedItem as DiplomaticRelationDisplay;
                                if (selectedItem.LinkedFaction1 == this.EditingFaction)
                                {
                                    this.EditingSection.OrientationFaction = selectedItem.LinkedFaction2;
                                }
                                else if (selectedItem.LinkedFaction2 == this.EditingFaction)
                                {
                                    this.EditingSection.OrientationFaction = selectedItem.LinkedFaction1;
                                }
                                this.RefreshOKButton();
                                this.RefreshLabelTextsDisplay();
                            };
                        }
                        this.GameFramePlugin.SetOKFunction(function2);
                        break;

                    case SectionOrientationKind.州域:
                        this.TabListPlugin.InitialValues(Session.Current.Scenario.States.GetList(), null, InputManager.NowMouse.ScrollWheelValue, "");
                        this.TabListPlugin.SetListKindByName("State", true, false);
                        this.TabListPlugin.SetSelectedTab("");
                        this.GameFramePlugin.Kind = FrameKind.SectionAIDetail;
                        this.GameFramePlugin.Function = FrameFunction.GetFaction;
                        this.GameFramePlugin.SetFrameContent(this.TabListPlugin.TabList, Session.MainGame.mainGameScreen.viewportSizeFull);
                        this.GameFramePlugin.OKButtonEnabled = false;
                        this.GameFramePlugin.CancelButtonEnabled = true;
                        if (function3 == null)
                        {
                            function3 = delegate
                            {
                                this.EditingSection.OrientationFaction = null;
                                this.EditingSection.OrientationSection = null;
                                this.EditingSection.OrientationState = this.TabListPlugin.SelectedItem as State;
                                this.EditingSection.OrientationArchitecture = null;
                                this.RefreshOKButton();
                                this.RefreshLabelTextsDisplay();
                            };
                        }
                        this.GameFramePlugin.SetOKFunction(function3);
                        break;

                    case SectionOrientationKind.建筑:
                        ArchitectureList allArch = Session.Current.Scenario.Architectures;
                        ArchitectureList targetArch = new ArchitectureList();
                        foreach (Architecture a in allArch)
                        {
                            if (a.BelongedFaction != this.EditingFaction)
                            {
                                targetArch.Add(a);
                            }
                        }
                        this.TabListPlugin.InitialValues(targetArch, null, InputManager.NowMouse.ScrollWheelValue, "");
                        this.TabListPlugin.SetListKindByName("Architecture", true, false);
                        this.TabListPlugin.SetSelectedTab("");
                        this.GameFramePlugin.Kind = FrameKind.Architecture;
                        this.GameFramePlugin.Function = FrameFunction.GetFaction;
                        this.GameFramePlugin.SetFrameContent(this.TabListPlugin.TabList, Session.MainGame.mainGameScreen.viewportSizeFull);
                        this.GameFramePlugin.OKButtonEnabled = false;
                        this.GameFramePlugin.CancelButtonEnabled = true;
                        if (function3 == null)
                        {
                            function3 = delegate
                            {
                                this.EditingSection.OrientationFaction = null;
                                this.EditingSection.OrientationSection = null;
                                this.EditingSection.OrientationState = null;
                                this.EditingSection.OrientationArchitecture = this.TabListPlugin.SelectedItem as Architecture;
                                this.RefreshOKButton();
                                this.RefreshLabelTextsDisplay();
                            };
                        }
                        this.GameFramePlugin.SetOKFunction(function3);
                        break;
                }
                this.GameFramePlugin.IsShowing = true;
            }
            catch (Exception ex)
            {
                WebTools.TakeWarnMsg("MarshalSectionDialog.ShowOrientationFrame", "", ex);
            }
        }

        internal void Update()
        {
        }

        private Rectangle AIDetailButtonDisplayPosition
        {
            get
            {
                return new Rectangle(this.DisplayOffset.X + this.AIDetailButtonPosition.X, this.DisplayOffset.Y + this.AIDetailButtonPosition.Y, this.AIDetailButtonPosition.Width, this.AIDetailButtonPosition.Height);
            }
        }

        private Rectangle ArchitectureListButtonDisplayPosition
        {
            get
            {
                return new Rectangle(this.DisplayOffset.X + this.ArchitectureListButtonPosition.X, this.DisplayOffset.Y + this.ArchitectureListButtonPosition.Y, this.ArchitectureListButtonPosition.Width, this.ArchitectureListButtonPosition.Height);
            }
        }

        private Rectangle BackgroundDisplayPosition
        {
            get
            {
                return new Rectangle(this.DisplayOffset.X, this.DisplayOffset.Y, this.BackgroundSize.X, this.BackgroundSize.Y);
            }
        }

        private Rectangle CancelButtonDisplayPosition
        {
            get
            {
                return new Rectangle(this.DisplayOffset.X + this.CancelButtonPosition.X, this.DisplayOffset.Y + this.CancelButtonPosition.Y, this.CancelButtonPosition.Width, this.CancelButtonPosition.Height);
            }
        }

        public bool IsShowing
        {
            get
            {
                return this.isShowing;
            }
            set
            {
                this.isShowing = value;
                if (value)
                {
                    Session.MainGame.mainGameScreen.PushUndoneWork(new UndoneWorkItem(UndoneWorkKind.Dialog, UndoneWorkSubKind.None));
                    Session.MainGame.mainGameScreen.OnMouseLeftUp += new Screen.MouseLeftUp(this.screen_OnMouseLeftUp);
                    Session.MainGame.mainGameScreen.OnMouseMove += new Screen.MouseMove(this.screen_OnMouseMove);
                    Session.MainGame.mainGameScreen.OnMouseRightDown += new Screen.MouseRightDown(this.screen_OnMouseRightDown);
                }
                else
                {
                    if (Session.MainGame.mainGameScreen.PopUndoneWork().Kind != UndoneWorkKind.Dialog)
                    {
                        throw new Exception("The UndoneWork is not a TransportDialog.");
                    }
                    Session.MainGame.mainGameScreen.OnMouseLeftUp -= new Screen.MouseLeftUp(this.screen_OnMouseLeftUp);
                    Session.MainGame.mainGameScreen.OnMouseMove -= new Screen.MouseMove(this.screen_OnMouseMove);
                    Session.MainGame.mainGameScreen.OnMouseRightDown -= new Screen.MouseRightDown(this.screen_OnMouseRightDown);
                }
            }
        }

        private Rectangle OKButtonDisplayPosition
        {
            get
            {
                return new Rectangle(this.DisplayOffset.X + this.OKButtonPosition.X, this.DisplayOffset.Y + this.OKButtonPosition.Y, this.OKButtonPosition.Width, this.OKButtonPosition.Height);
            }
        }

        private Rectangle OrientationButtonDisplayPosition
        {
            get
            {
                return new Rectangle(this.DisplayOffset.X + this.OrientationButtonPosition.X, this.DisplayOffset.Y + this.OrientationButtonPosition.Y, this.OrientationButtonPosition.Width, this.OrientationButtonPosition.Height);
            }
        }
    }
}

